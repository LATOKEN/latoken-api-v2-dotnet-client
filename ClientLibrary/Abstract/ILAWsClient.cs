using Latoken.Api.Client.Library.Dto.WS;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;


namespace Latoken.Api.Client.Library.Abstract
{
    /// <summary>
    ///     This interface used to work with web-sockets connection
    /// </summary>
    public interface ILAWsClient
    {
        event Action OnOpened;
        event Action<WebSocketCloseStatus?, string> OnClosed;
        event Action<string, string> OnError;

        /// <summary>
        ///     This method returns true if the STOMP connection established
        /// </summary>
        bool IsStompConnected();

        /// <summary>
        ///     This method shold be called to establish connectoin
        /// </summary>
        void Start();

        /// <summary>
        ///     This method should be called to close connection
        /// </summary>
        void Dispose();

        void SubscribeBalanceEvents(Action<List<Balance>, DateTime> balancesCallback);
        void SubscribeOrderEvents(Action<List<Order>, DateTime> ordersCallback);
        void SubscribeMyTradeEvents(string baseId, string quoteId, Action<List<MyTrade>, DateTime> tradesCallback);
        void SubscribeOrderBookEvents(string baseId, string quoteId, Action<OrderBookChange, string, string, DateTime> orderbooksCallback);
    }
}
