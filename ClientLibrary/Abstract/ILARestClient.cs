using Latoken_CSharp_Client_Library.Commands;
using Latoken_CSharp_Client_Library.Constants;
using Latoken_CSharp_Client_Library.Dto.Rest;
using Latoken_CSharp_Client_Library.Utils.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Latoken_CSharp_Client_Library
{
    public interface ILARestClient
    {
        void SetCredentials(ClientCredentials client);
        Task<OrderBook> GetOrderBook(string baseCurrency, string quoteCurrency, int limit);
        Task<List<Pair>> GetPairs();
        Task<Pair> GetPair(string baseCurrency, string quoteCurrency);
        Task<List<Pair>> GetAvailablePairs();
        Task<Pair> GetAvailablePair(string baseCurrency, string quoteCurrency);
        Task<Pair> GetPairByCurrencyId(string baseCurrencyId, string quoteCurrencyId);
        Task<Currency> GetCurrency(string currency);
        Task<List<Currency>> GetCurrencies();
        Task<List<Ticker>> GetTickers();
        Task<Ticker> GetTicker(string baseCurrency, string quoteCurrency);
        Task<Rate> GetRate(string baseCurrency, string quoteCurrency);
        Task<List<Trade>> GetAllTrades(string baseCurrency, string quoteCurrency);
        Task<List<Trade>> GetClientTrades(int page, int size);
        Task<List<Trade>> GetClientTradesPair(string baseCurrency, string quoteCurrency, int page, int size);
        Task<List<LatokenLatoken_CSharp_Client_Library.Order>> GetOrders(int size);
        Task<List<LatokenLatoken_CSharp_Client_Library.Order>> GetOrdersPair(string baseCurrency, string quoteCurrency, int page, int size);
        Task<OrderResponse> PlaceOrder(OrderCommand command);
        Task<OrderResponse> CancelOrder(OrderIdCommand command);
        Task<LatokenLatoken_CSharp_Client_Library.Order> GetOrder(OrderIdCommand command);
        Task<List<Balance>> GetBalances(bool zeros);
        Task<CancelAllOrdersResponce> CancelAllOrders(string baseCurrency, string quoteCurrency);
        Task<LatokenUser> GetUser();        
        Task<Balance> GetBalanceByType(string currencyId, TypeOfAccount typeOfAccount);

        bool IsReady();
        Task<Klines> GetKlines(string baseCurrency, string quoteCurrency, int intervalMin, long from, long to);
    }
}
