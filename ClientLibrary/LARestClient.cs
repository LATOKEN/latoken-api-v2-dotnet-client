using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Latoken_CSharp_Client_Library.Commands;
using Latoken_CSharp_Client_Library.Constants;
using Latoken_CSharp_Client_Library.Dto.Rest;
using Latoken_CSharp_Client_Library.LA.Commands;
using Latoken_CSharp_Client_Library.Utils.Configuration;
using LatokenLatoken_CSharp_Client_Library;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Latoken_CSharp_Client_Library
{
    public class LARestClient : ILARestClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LARestClient> _logger;
        private readonly ApiErrorParser _errorParser = new ApiErrorParser();
        private ClientCredentials _client;

        public LARestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // NOTE: latoken exchange will return 500 if response time exceeded  13 sec
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }
        
        public LARestClient(HttpClient httpClient, ILogger<LARestClient> logger) : this(httpClient)
        {
            _logger = logger;
        }

        public void SetCredentials(ClientCredentials client)
        {
            _client = client;
        }

        public bool IsReady()
        {
            return _client != null;
        }

        public async Task<OrderBook> GetOrderBook(string baseCurrency, string quoteCurrency, int limit = 100)
        {
            return await Get<OrderBook>(ApiPath.GetOrderBook(baseCurrency, quoteCurrency, limit));
        }

        public async Task<List<Pair>> GetPairs()
        {
            return await Get<List<Pair>>(ApiPath.GetPairs);
        }

        public async Task<List<Pair>> GetAvailablePairs()
        {
            return await Get<List<Pair>>(ApiPath.GetAvailablePairs);
        }

        public async Task<Pair> GetPair(string baseCurrency, string quoteCurrency)
        {
            var pairs = await GetPairs();
            var currencyBase = await GetCurrency(baseCurrency.ToUpper());
            var currencyQuote = await GetCurrency(quoteCurrency.ToUpper());
            return pairs.FirstOrDefault(x => x.BaseCurrency == currencyBase.Id && x.QuoteCurrency == currencyQuote.Id);
        }

        public async Task<Pair> GetAvailablePair(string baseCurrency, string quoteCurrency)
        {
            var pairs = await GetAvailablePairs();
            var currencyBase = await GetCurrency(baseCurrency.ToUpper());
            var currencyQuote = await GetCurrency(quoteCurrency.ToUpper());
            return pairs.First(x => x.BaseCurrency == currencyBase.Id && x.QuoteCurrency == currencyQuote.Id);
        }


        public async Task<Pair> GetPairByCurrencyId(string baseCurrencyId, string quoteCurrencyId)
        {
            var pairs = await GetPairs();
            return pairs.FirstOrDefault(x => x.BaseCurrency == baseCurrencyId && x.QuoteCurrency == quoteCurrencyId);
        }

        public async Task<Currency> GetCurrency(string currency)
        {
            return await Get<Currency>(ApiPath.GetCurrency(currency.ToUpper()));
        }

        public async Task<List<Currency>> GetCurrencies()
        {
            return await Get<List<Currency>>(ApiPath.GetCurrencies);
        }

        public async Task<List<Ticker>> GetTickers()
        {
            return await Get<List<Ticker>>(ApiPath.GetTickers);
        }

        public async Task<Ticker> GetTicker(string baseCurrency, string quoteCurrency)
        {
            return await Get<Ticker>(ApiPath.GetTicker(baseCurrency.ToUpper(), quoteCurrency.ToUpper()));
        }

        public async Task<Rate> GetRate(string baseCurrency, string quoteCurrency)
        {
            return await Get<Rate>(ApiPath.GetRate(baseCurrency, quoteCurrency));
        }

        public async Task<List<Trade>> GetAllTrades(string baseCurrency, string quoteCurrency)
        {
            return await Get<List<Trade>>(ApiPath.GetAllTrades(baseCurrency.ToUpper(), quoteCurrency.ToUpper()));
        }

        public async Task<List<Trade>> GetClientTrades(int page = 0, int size = 20)
        {
            return await Get<List<Trade>>(ApiPath.GetClientTrades(page, size), true);
        }

        public async Task<List<Trade>> GetClientTradesPair(string baseCurrency, string quoteCurrency, int page = 0, int size = 20)
        {
            return await Get<List<Trade>>(ApiPath.GetClientTradesPair(baseCurrency.ToUpper(), quoteCurrency.ToUpper(), page, size), true);
        }

        public async Task<List<Order>> GetOrders(int size = 100)
        {
            return await Get<List<Order>>(ApiPath.GetOrders(size), true);
        }

        public async Task<List<Order>> GetOrdersPair(string baseCurrency, string quoteCurrency, int page = 0, int size = 20)
        {
            var uri = ApiPath.GetOrdersPair(baseCurrency.ToUpper(), quoteCurrency.ToUpper(), page, size);
            return await Get<List<Order>>(uri, true);
        }

        public async Task<OrderResponse> PlaceOrder(OrderCommand command)
        {
            var res = await PostSafe<OrderResponse>(ApiPath.PlaceOrder, command, true);
            res.Content.ErrorCode = res.Code;
            return res.Content;
        }

        public async Task<OrderResponse> CancelOrder(OrderIdCommand command)
        {
            var res = await PostSafe<OrderResponse>(ApiPath.CancelOrder, command, true);
            res.Content.ErrorCode = res.Code;
            return res.Content;
        }

        public async Task<Order> GetOrder(OrderIdCommand command)
        {
            return await Get<Order>(ApiPath.GetOrder(command.Id), true);
        }

        public async Task<Transaction> TransferInternal(TransferCommand command)
        {
            return await Post<Transaction>(ApiPath.TransferByUserId, command, true);
        }

        public async Task<Transaction> SpotWithdraw(SpotTransferCommand command)
        {
            return await Post<Transaction>(ApiPath.SpotWithdraw, command, true);
        }

        public async Task<List<Balance>> GetBalances(bool zeros = true)
        {
            return await Get<List<Balance>>(ApiPath.GetBalances(zeros), true);
        }

        public async Task<LatokenUser> GetUser()
        {
            return await Get<LatokenUser>(ApiPath.GetUser, true);
        }

        public async Task<CancelAllOrdersResponce> CancelAllOrders(string baseCurrency, string quoteCurrency)
        {
            var res =await PostSafe<CancelAllOrdersResponce>(ApiPath.CancelAllOrders(baseCurrency, quoteCurrency), null, true);
            return res.Content;
        }
        

        public async Task<Balance> GetBalanceByType(string currency, TypeOfAccount typeOfAccount)
        {
            return await Get<Balance>(ApiPath.GetBalancesByType(currency, typeOfAccount.ToString()), true);
        }

        public async Task<Klines> GetKlines(string baseCurrency, string quoteCurrency, int intervalMin, long from, long to)
        {
            var symbol = HttpUtility.UrlEncode(baseCurrency + "/" + quoteCurrency);
            return await Get<Klines>(ApiPath.GetKlines(symbol, intervalMin, from, to));
        }

        private async Task<T> Get<T>(string url, bool auth = false)
        {
            var response = await PerformRequest(HttpMethod.Get, url, auth);
            return await DeserializeJsonFromStream<T>(response);
        }

        [Obsolete("Consider using error response instead of success response and exception. Use PostSafe")]
        private async Task<T> Post<T>(string url, object body, bool auth = false)
        {
            var response = await PerformRequest(HttpMethod.Post, url, auth, body);
            return await DeserializeJsonFromStream<T>(response);
        }
        
        private async Task<PostSafeResponse<T>> PostSafe<T>(string url, object body, bool auth = false)
        {
            var response = await PerformRequest(HttpMethod.Post, url, auth, body);
            var content = await DeserializeJsonFromStream<T>(response, false);
            var errorCode = await _errorParser.GetErrorCode(response);
            return new PostSafeResponse<T>(content, errorCode);
        }

        private async Task<HttpResponseMessage> PerformRequest(HttpMethod httpMethod, string url, bool auth = false, object body = null)
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

            var response = await _httpClient.SendAsync(request);
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

        private class PostSafeResponse<T>
        {
            public PostSafeResponse(T content, ApiResponseCode code)
            {
                Content = content;
                Code = code;
            }
            public T Content { get; }
            public ApiResponseCode Code { get; }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }        
    }
}
