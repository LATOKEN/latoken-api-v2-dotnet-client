using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Latoken.Api.Client.Library.Commands;
using Latoken.Api.Client.Library.Constants;
using Latoken.Api.Client.Library.Dto.Rest;
using Latoken.Api.Client.Library.LA.Commands;
using Latoken.Api.Client.Library.Utils.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Latoken.Api.Client.Library
{
    public class LARestClient : ILARestClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LARestClient> _logger;
        private ClientCredentials _client;

        public LARestClient(ClientCredentials client, HttpClient httpClient)
        {
            _client = client;
            _httpClient = httpClient;
            // NOTE: latoken exchange will return 500 if response time exceeded  13 sec
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }
        
        public LARestClient(ClientCredentials client, 
                            HttpClient httpClient, 
                            ILogger<LARestClient> logger) : this(client, httpClient)
        {
            _logger = logger;
        }

        public bool IsReady()
        {
            return _client != null;
        }

        public Task<OrderBook> GetOrderBook(string baseCurrency, string quoteCurrency, int limit = 100)
        {
            var task = Get<OrderBook>(ApiPath.GetOrderBook(baseCurrency, quoteCurrency, limit));
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Pair>> GetPairs()
        {
            var task = Get<List<Pair>>(ApiPath.GetPairs);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Pair>> GetAvailablePairs()
        {
            var task = Get<List<Pair>>(ApiPath.GetAvailablePairs);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<Currency> GetCurrency(string currency)
        {
            var task = Get<Currency>(ApiPath.GetCurrency(currency.ToUpper()));
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Currency>> GetCurrencies()
        {
            var task = Get<List<Currency>>(ApiPath.GetCurrencies);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Ticker>> GetTickers()
        {
            var task = Get<List<Ticker>>(ApiPath.GetTickers);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<Ticker> GetTicker(string baseCurrency, string quoteCurrency)
        {
            var task = Get<Ticker>(ApiPath.GetTicker(baseCurrency.ToUpper(), quoteCurrency.ToUpper()));
            task.ConfigureAwait(false);
            return task;
        }

        public Task<Rate> GetRate(string baseCurrency, string quoteCurrency)
        {
            var task = Get<Rate>(ApiPath.GetRate(baseCurrency, quoteCurrency));
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Trade>> GetAllTrades(string baseCurrency, string quoteCurrency)
        {
            var task = Get<List<Trade>>(ApiPath.GetAllTrades(baseCurrency.ToUpper(), quoteCurrency.ToUpper()));
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Trade>> GetClientTrades(int page = 0, int size = 20)
        {
            var task = Get<List<Trade>>(ApiPath.GetClientTrades(page, size), true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Trade>> GetClientTradesPair(string baseCurrency, string quoteCurrency, int page = 0, int size = 20)
        {
            var task = Get<List<Trade>>(ApiPath.GetClientTradesPair(baseCurrency.ToUpper(), quoteCurrency.ToUpper(), page, size), true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Order>> GetOrders(int size = 100)
        {
            var task = Get<List<Order>>(ApiPath.GetOrders(size), true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Order>> GetOrdersPair(string baseCurrency, string quoteCurrency, int page = 0, int size = 20)
        {
            var uri = ApiPath.GetOrdersPair(baseCurrency.ToUpper(), quoteCurrency.ToUpper(), page, size);
            var task = Get<List<Order>>(uri, true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<OrderResponse> PlaceOrder(OrderCommand command)
        {
            var task = Post<OrderResponse>(ApiPath.PlaceOrder, command, true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<OrderResponse> CancelOrder(OrderIdCommand command)
        {
            var task = Post<OrderResponse>(ApiPath.CancelOrder, command, true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<Order> GetOrder(OrderIdCommand command)
        {
            var task = Get<Order>(ApiPath.GetOrder(command.Id), true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<Transaction> TransferInternal(TransferCommand command)
        {
            var task = Post<Transaction>(ApiPath.TransferByUserId, command, true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<Transaction> SpotWithdraw(SpotTransferCommand command)
        {
            var task = Post<Transaction>(ApiPath.SpotWithdraw, command, true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<List<Balance>> GetBalances(bool zeros = true)
        {
            var task = Get<List<Balance>>(ApiPath.GetBalances(zeros), true);
            task.ConfigureAwait(false);

            return task;
        }

        public Task<LatokenUser> GetUser()
        {
            var task = Get<LatokenUser>(ApiPath.GetUser, true);
            task.ConfigureAwait(false);
            return task;
        }

        public Task<CancelAllOrdersResponce> CancelAllOrders(string baseCurrency, string quoteCurrency)
        {
            var task = Post<CancelAllOrdersResponce>(ApiPath.CancelAllOrders(baseCurrency, quoteCurrency), null, true);
            task.ConfigureAwait(false);
            return task;
        }
        

        public Task<Balance> GetBalanceByType(string currency, TypeOfAccount typeOfAccount)
        {
            var task = Get<Balance>(ApiPath.GetBalancesByType(currency, typeOfAccount.ToString()), true);
            task.ConfigureAwait(false);
            return task;
        }

        private Task<T> Get<T>(string url, bool auth = false)
        {
            var response = PerformRequest(HttpMethod.Get, url, auth);
            return DeserializeJsonFromStream<T>(response.Result);
        }

        private Task<T> Post<T>(string url, object body, bool auth = false)
        {
            var response =  PerformRequest(HttpMethod.Post, url, auth, body);
            return DeserializeJsonFromStream<T>(response.Result);
        }
        
        private Task<HttpResponseMessage> PerformRequest(HttpMethod httpMethod, string url, bool auth = false, object body = null)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            var splitUrl = url.Split("?");
            var path = splitUrl[0];
            var getParams = string.Empty;

            if (splitUrl.Length > 1)
                getParams = splitUrl[1];

            var postParams = string.Empty;
            
            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, LAHeaders.MediaType);
                request.Content = content;
                
                var properties = from p in body.GetType().GetProperties()
                    where p.GetValue(body, null) != null
                    select p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName + "=" +
                           HttpUtility.UrlEncode(p.GetValue(body, null).ToString());
                postParams = string.Join("&", properties.ToArray());
            }

            if (auth)
            {
                request.Headers.Add(LAHeaders.LA_APIKEY, _client.ApiKey);
                path = new Uri(_httpClient.BaseAddress, path).AbsolutePath;
                request.Headers.Add(LAHeaders.LA_SIGNATURE, SignatureService.CreateSignature(_client.ApiSecret, httpMethod + path + getParams + postParams));
                request.Headers.Add(LAHeaders.LA_DIGEST, LAHeaders.HASH_ALGO);    
            }

            var response = _httpClient.SendAsync(request);
            return response;
        }
        
        private async Task<T> DeserializeJsonFromStream<T>(HttpResponseMessage response, bool throwErrors = true)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            
            LogErrorResponse(response);
            
            var content = await response.Content?.ReadAsStringAsync();

            if(!throwErrors)
                return JsonConvert.DeserializeObject<T>(content);
            
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(content);    
            }
            
            throw new Exception(content);
        }

        private void LogErrorResponse(HttpResponseMessage response)
        {
            
            if(response.IsSuccessStatusCode) return;
            
            _logger?.LogError(
                $"[{response.RequestMessage.Method.Method}] {response.RequestMessage.RequestUri} {(int)response.StatusCode} {response.ReasonPhrase} {response.Content?.ReadAsStringAsync().Result}");
                               
        }


        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }        
    }
}
