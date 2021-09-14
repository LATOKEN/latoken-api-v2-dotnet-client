using Latoken.Api.Client.Library.Commands;
using Latoken.Api.Client.Library.Constants;
using Latoken.Api.Client.Library.Dto.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Latoken.Api.Client.Library
{
    /// <summary>
    ///     This interface used to work with REST connection
    /// </summary>
    public interface ILARestClient
    {
        Task<OrderBook> GetOrderBook(string baseCurrency, string quoteCurrency, int limit);
        Task<List<Pair>> GetPairs();
        Task<List<Pair>> GetAvailablePairs();
        Task<Currency> GetCurrency(string currency);
        Task<List<Currency>> GetCurrencies();
        Task<List<Ticker>> GetTickers();
        Task<Ticker> GetTicker(string baseCurrency, string quoteCurrency);
        Task<Rate> GetRate(string baseCurrency, string quoteCurrency);
        Task<List<Trade>> GetAllTrades(string baseCurrency, string quoteCurrency);
        Task<List<Trade>> GetClientTrades(int page, int size);
        Task<List<Trade>> GetClientTradesPair(string baseCurrency, string quoteCurrency, int page, int size);
        Task<List<Order>> GetOrders(int size);
        Task<List<Order>> GetOrdersPair(string baseCurrency, string quoteCurrency, int page, int size);
        Task<OrderResponse> PlaceOrder(OrderCommand command);
        Task<OrderResponse> CancelOrder(OrderIdCommand command);
        Task<Order> GetOrder(OrderIdCommand command);
        Task<List<Balance>> GetBalances(bool zeros);
        Task<CancelAllOrdersResponce> CancelAllOrders(string baseCurrency, string quoteCurrency);
        Task<LatokenUser> GetUser();        
        Task<Balance> GetBalanceByType(string currencyId, TypeOfAccount typeOfAccount);

        /// <summary>
        ///     Returns true if the REST client is ready to establish a connection
        /// </summary>
        bool IsReady();
    }
}
