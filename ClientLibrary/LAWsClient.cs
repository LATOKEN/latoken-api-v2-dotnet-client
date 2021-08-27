using Latoken_CSharp_Client_Library.Abstract;
using Latoken_CSharp_Client_Library.Commands.Stomp;
using Latoken_CSharp_Client_Library.Constants;
using Latoken_CSharp_Client_Library.Dto.WS;
using Latoken_CSharp_Client_Library.Utils.Configuration;
using LatokenLatoken_CSharp_Client_Library;
using LatokenLatoken_CSharp_Client_Library.Dto.WS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using Websocket.Client.Models;

namespace Latoken_CSharp_Client_Library
{
    public class LAWsClient : ILAWsClient, IDisposable
    {
        private WebsocketClient _webSocket;        
        private ClientCredentials _client;
        private bool _isDisposed;
        private int _stompConnected;
        private Dictionary<string, object> _dataHandlers = new Dictionary<string, object>();
        private Dictionary<string, int> _subscriptionNonces = new Dictionary<string, int>();
        private long _lastMessageTimestamp = DateTime.UnixEpoch.Ticks;
        private CancellationTokenSource _heartbeatStoppingToken;

        public event Action OnOpened;
        public event Action<WebSocketCloseStatus?, string> OnClosed;
        public event Action<string, string> OnError;

        private const int HEARTBEAT_INTERVAL_MS = 10000;

        public LAWsClient(ClientCredentials client = null, bool autoReconnect = true)
        {            
            //should be 2 times more than stomp heartbeat delay 
            var timeout = TimeSpan.FromMilliseconds(HEARTBEAT_INTERVAL_MS);
            var factory = new Func<ClientWebSocket>(() => new ClientWebSocket
            {
                Options =
                {
                    KeepAliveInterval = timeout,
                }
            });
            Console.WriteLine($"Trying to establish web socket connection to {ApiPath.LAWSApiUrl}");
            _webSocket = new WebsocketClient(new Uri(ApiPath.LAWSApiUrl), factory);
            SetCredentials(client);
            _webSocket.ReconnectTimeout = timeout;
            _webSocket.ErrorReconnectTimeout = TimeSpan.FromMilliseconds(HEARTBEAT_INTERVAL_MS * 2);
            _webSocket.IsReconnectionEnabled = autoReconnect;
            _webSocket.DisconnectionHappened.Subscribe(WS_OnDisconnect);
            _webSocket.ReconnectionHappened.Subscribe(WS_OnReconnect);
            _webSocket.MessageReceived.Subscribe(WS_OnMessage);
            //this method will not throw an exception
            _webSocket.Start();
        }

        private void SetCredentials(ClientCredentials client)
        {
            if (client != null)
            {
                _client = client;
                _webSocket.Name = client.UserId;
                Console.WriteLine($"Client {client.UserId} credentials for web socket has been set.");
            }
        }

        private bool TryGetUTF8DecodedString(byte[] bytes, out string s)
        {
            s = null;

            try
            {
                s = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void WS_OnReconnect(ReconnectionInfo info)
        {
            Console.WriteLine($"Reconnection happened with type: {info.Type}");

            if (_client != null)
                _webSocket.Name = _client.UserId;

            //re-subscribe
            //do not do this on the initial connection establishing for further manual control 
            if (info.Type != ReconnectionType.Initial)
                Start();
        }

        private void WS_OnDisconnect(DisconnectionInfo info)
        {
            Interlocked.Exchange(ref _stompConnected, 0);
            CleanWSData();
            OnClosed?.Invoke(info.CloseStatus, info.CloseStatusDescription);
            Console.WriteLine($"Disconnection happened with type: {info.Type}, status: {info.CloseStatus}, " +
                                    $"description: {info.CloseStatusDescription}");

            if (info.Exception != null)
                Console.WriteLine("Disconnection happened with the error!");
        }

        private void WS_OnMessage(ResponseMessage message)
        {
            string data = null;

            if (message.MessageType == WebSocketMessageType.Binary)
                TryGetUTF8DecodedString(message.Binary, out data);

            if (message.MessageType == WebSocketMessageType.Text)
                data = message.Text;

            if (data != null)
            {
                Interlocked.Exchange(ref _lastMessageTimestamp, DateTime.UtcNow.Ticks);
                var frame = StompMessageSerializer.Deserialize(data);

                switch (frame.Command)
                {
                    case StompFrame.CONNECTED:
                        {
                            Interlocked.Exchange(ref _stompConnected, 1);
                            OnOpened?.Invoke();
                            Console.WriteLine($"Latoken web socket stomp client connected successfully.");
                            _heartbeatStoppingToken = new CancellationTokenSource();
                            HeartBeat(_heartbeatStoppingToken.Token).ContinueWith(t =>
                                Console.WriteLine("Heartbeat process faulted with error! Exception: {0}", t.Exception),
                                TaskContinuationOptions.OnlyOnFaulted);
                        }
                        break;
                    case StompFrame.MESSAGE:
                        {
                            Console.WriteLine($"Latoken web socket message acquired: '{frame.Body}'");

                            if (frame.Headers.ContainsKey(LAHeaders.SUBSCRIPTION_HEADER))
                            {
                                var subscriptionId = frame.Headers[LAHeaders.SUBSCRIPTION_HEADER];

                                if (_dataHandlers.ContainsKey(subscriptionId))
                                {
                                    var subscrParts = subscriptionId.Split(ApiPath.STOMP_SEPARATOR);
                                    //We have to free web socket execution thread as fast as possible
                                    //client should consider to use multithreading/parallelizing in case of heavy task
                                    switch (subscrParts[0])
                                    {
                                        case LAHeaders.ACCOUNT_ID:
                                            {
                                                (_dataHandlers[subscriptionId] as Action<List<Balance>, DateTime>)?
                                                    .Invoke(DeserializeJsonFromMessage<List<Balance>>(frame.Body, subscriptionId, out var timestamp), timestamp);
                                            }
                                            break;
                                        case LAHeaders.ORDER_ID:
                                            {
                                                (_dataHandlers[subscriptionId] as Action<List<Order>, DateTime>)?
                                                    .Invoke(DeserializeJsonFromMessage<List<Order>>(frame.Body, subscriptionId, out var timestamp), timestamp);
                                            }
                                            break;
                                        case LAHeaders.MY_TRADES_ID:
                                            {
                                                (_dataHandlers[subscriptionId] as Action<List<MyTrade>, DateTime>)?
                                                    .Invoke(DeserializeJsonFromMessage<List<MyTrade>>(frame.Body, subscriptionId, out var timestamp), timestamp);
                                            }
                                            break;
                                        case LAHeaders.ORDER_BOOK_ID:
                                            {
                                                (_dataHandlers[subscriptionId] as Action<OrderBookChange, string, string, DateTime>)?
                                                    .Invoke(DeserializeJsonFromMessage<OrderBookChange>(frame.Body, subscriptionId, out var timestamp),
                                                                    subscrParts[1], subscrParts[2], timestamp);
                                            }
                                            break;
                                        default:
                                            Console.WriteLine($"Stomp message '{frame.Body}' received for unknown subscription id {subscriptionId} !");
                                            break;
                                    }
                                }
                                else
                                    Console.WriteLine($"Stomp message '{frame.Body}' received for unsubscribed id {subscriptionId} !");
                            }
                            else
                                Console.WriteLine($"Stomp message '{frame.Body}' doesn't belong to subscription type");
                        }
                        break;
                    case StompFrame.ERROR:
                        {
                            //TODO parse other error specific headers if applicable
                            OnError?.Invoke(frame.Headers[LAHeaders.MESSAGE_HEADER], frame.Body);
                            Console.WriteLine($"Latoken web socket stomp error: '{frame.Body}' received!");
                        }
                        break;
                    default:
                        //this is ping
                        if (frame.Body != "\n")
                            Console.WriteLine($"Latoken web socket unknown stomp command '{frame.Command}' received, content: '{frame.Body}'");
                        break;
                }
            }
        }

        public bool IsStompConnected()
        {
            return Interlocked.CompareExchange(ref _stompConnected, -1, -1) == 1;
        }

        private bool CheckStompIsAlive()
        {
            if (CheckWSIsAlive())
                if (IsStompConnected())
                    return true;

            OnError?.Invoke(LAHeaders.STOMP_NOT_ALIVE, string.Empty);
            Console.WriteLine(LAHeaders.STOMP_NOT_ALIVE);
            return false;
        }

        private bool CheckWSIsAlive()
        {
            if (_webSocket != null && _webSocket.IsRunning)
                return true;

            OnError?.Invoke(LAHeaders.WS_NOT_ALIVE, string.Empty);
            Console.WriteLine(LAHeaders.WS_NOT_ALIVE);
            return false;
        }

        
        public void Start()
        {
            if (CheckWSIsAlive())
            {
                if (!IsStompConnected())
                {
                    //values from Latoken WS api doc
                    var version = "1.0,1.1,1.2";
                    var heartbeat = $"{HEARTBEAT_INTERVAL_MS},{HEARTBEAT_INTERVAL_MS}";

                    if (_client != null)
                        _webSocket.Send(StompMessageSerializer.Serialize(StompMessage.CreateSignedConnectMessage(version, heartbeat, _client.ApiKey, _client.ApiSecret)));
                    else
                        _webSocket.Send(StompMessageSerializer.Serialize(StompMessage.CreateConnectMessage(version, heartbeat)));
                }
                else
                    Console.WriteLine("Stomp has been connected already;");
            }
        }

        private void SubscribeStream(StompMessage subscriptionMessage, object streamEventsHandler)
        {
            if (CheckStompIsAlive())
            {
                var id = subscriptionMessage.Headers[LAHeaders.ID_HEADER];
                _webSocket.Send(StompMessageSerializer.Serialize(subscriptionMessage));
                _dataHandlers[id] = streamEventsHandler;
                _subscriptionNonces[id] = 0;
            }
        }

        public void SubscribeBalanceEvents(Action<List<Balance>, DateTime> balancesCallback)
        {
            SubscribeStream(StompMessage.CreateSubsribeMessage(
                                        ApiPath.LAWSAccount(_client.UserId, out var id), id), balancesCallback);
        }

        public void SubscribeOrderEvents(Action<List<Order>, DateTime> ordersCallback)
        {
            SubscribeStream(StompMessage.CreateSubsribeMessage(
                                        ApiPath.LAWSOrders(_client.UserId, out var id), id), ordersCallback);
        }

        public void SubscribeMyTradeEvents(string baseId, string quoteId, Action<List<MyTrade>, DateTime> tradesCallback)
        {
            SubscribeStream(StompMessage.CreateSubsribeMessage(
                                        ApiPath.LAWSMyTrades(_client.UserId, baseId, quoteId, out var id), id), tradesCallback);
        }

        public void SubscribeOrderBookEvents(string baseId, string quoteId, Action<OrderBookChange, string, string, DateTime> orderbooksCallback)
        {
            SubscribeStream(StompMessage.CreateSubsribeMessage(
                                        ApiPath.LAWSOrderBook(baseId, quoteId, out var id), id), orderbooksCallback);
        }

        private T DeserializeJsonFromMessage<T>(string message, string subscriptionId, out DateTime timestamp)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var subscrMessage = JsonConvert.DeserializeObject<SubscriptionMessage<T>>(message);
            timestamp = DateTime.UnixEpoch.AddMilliseconds(subscrMessage.Timestamp);

            if (_subscriptionNonces[subscriptionId] != subscrMessage.Nonce)
            {
                Console.WriteLine($"Stomp subscription {subscriptionId} expected nonce " +
                                    $"{_subscriptionNonces[subscriptionId]} mismatch with received {subscrMessage.Nonce} !");

                //pass subscriptionId for the manual resubscribing
                OnError?.Invoke(LAHeaders.NONCE_MISMATCH_HEADER, subscriptionId);
                _webSocket.Send(StompMessageSerializer.Serialize(StompMessage.CreateUnsubsribeMessage(subscriptionId)));
                _subscriptionNonces.Remove(subscriptionId);
                _dataHandlers.Remove(subscriptionId);

                return default(T);
            }

            ++_subscriptionNonces[subscriptionId];
            return subscrMessage.Payload;
        }

        private async Task HeartBeat(CancellationToken stopToken)
        {
            while ((DateTime.UtcNow - new DateTime(Interlocked.CompareExchange(ref _lastMessageTimestamp, 0, 0)))
                        .TotalMilliseconds <= HEARTBEAT_INTERVAL_MS)
            {
                if (!stopToken.IsCancellationRequested)
                {
                    //heartbeat itself
                    _webSocket.Send("\n");
                    //wait half time to send heartbeat
                    await Task.Delay(HEARTBEAT_INTERVAL_MS / 2, stopToken);
                }
                else
                    break;
            }

            if (!stopToken.IsCancellationRequested)
            {
                Console.WriteLine("No incoming messages from the server! Going to disconnect..");
                await _webSocket.Reconnect();
                OnError?.Invoke(LAHeaders.STOMP_NO_HEARTBEAT, "heartbeat interval: " + HEARTBEAT_INTERVAL_MS.ToString());
            }
        }

        private void CleanWSData()
        {
            if (_heartbeatStoppingToken != null &&
                !_heartbeatStoppingToken.IsCancellationRequested)
            {
                _heartbeatStoppingToken.Cancel();
                _heartbeatStoppingToken = null;
            }
            //regarding to the protocol description there is no need to do graceful shutdown 
            //until you need to ensure the server received all previous frames sent by the client
            _dataHandlers.Clear();
            _subscriptionNonces.Clear();
        }

        private void CloseWS()
        {
            if (_webSocket != null && _webSocket.IsRunning)
            {
                foreach (var subscriptionId in _subscriptionNonces.Keys)
                    _webSocket.Send(StompMessageSerializer.Serialize(StompMessage.CreateUnsubsribeMessage(subscriptionId)));

                _webSocket.Send(StompMessageSerializer.Serialize(StompMessage.CreateDisconnectMessage("bye-bye")));
                _webSocket.Stop(WebSocketCloseStatus.NormalClosure, "Trying to gracefully close web socket..");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            //free managed
            if (disposing)
            {
                CloseWS();
                _webSocket.Dispose();
                CleanWSData();
            }

            _isDisposed = true;
        }
    }
}
