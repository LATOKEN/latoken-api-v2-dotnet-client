namespace Latoken.Api.Client.Library.Constants
{
    public static class ApiPath
    {
        private static string s_restVersion = "v2";

        // BookController
        public static string GetOrderBook(string baseCurrency, string quoteCurrency, int limit) => $"/{s_restVersion}/book/{baseCurrency}/{quoteCurrency}?limit={limit}";

        // PairController
        public static string GetPairs = $"/{s_restVersion}/pair";

        public static string GetAvailablePairs = $"/{s_restVersion}/pair/available";

        // CurrencyController 
        public static string GetCurrency(string currency) => $"/{s_restVersion}/currency/{currency}";
        public static string GetCurrencies = $"/{s_restVersion}/currency";

        // TickerController
        public static string GetTickers = $"/{s_restVersion}/ticker";
        public static string GetTicker(string baseCurrency, string quoteCurrency) => $"/{s_restVersion}/ticker/{baseCurrency}/{quoteCurrency}";
        public static string GetRate(string baseCurrency, string quoteCurrency) => $"/{s_restVersion}/rate/{baseCurrency}/{quoteCurrency}";

        // TradeController Public
        public static string GetAllTrades(string baseCurrency, string quoteCurrency) => 
            $"{s_restVersion}/trade/history/{baseCurrency}/{quoteCurrency}";

        // TradeController Auth
        public static string GetClientTrades(int page, int size) => $"/{s_restVersion}/auth/trade?page={page}&size={size}";
        public static string GetClientTradesPair(string baseCurrency, string quoteCurrency, int page, int size) => 
            $"/{s_restVersion}/auth/trade/pair/{baseCurrency}/{quoteCurrency}?page={page}&size={size}";

        // OrderController Auth
        public static string GetOrders(int size) => $"/{s_restVersion}/auth/order?limit={size}";
        public static string GetOrdersPair(string baseCurrency, string quoteCurrency, int page, int size) => 
            $"{s_restVersion}/auth/order/pair/{baseCurrency}/{quoteCurrency}/active?size={size}&page={page}";

        public static string PlaceOrder = $"{s_restVersion}/auth/order/place";
        public static string CancelOrder = $"{s_restVersion}/auth/order/cancel";
        public static string GetOrder(string orderId) => $"/{s_restVersion}/auth/order/getOrder/{orderId}";
        public static string TransferByUserId = $"/{s_restVersion}/auth/transfer/id";
        public static string SpotWithdraw = "/v2/auth/transfer/spot/withdraw";
        public static string CancelAllOrders(string baseCurrency, string quoteCurrency) => $"{s_restVersion}/auth/order/cancelAll/{baseCurrency}/{quoteCurrency}";

        // AccountController Auth
        public static string GetBalances(bool zeros) => $"/{s_restVersion}/auth/account?zeros={zeros}";
        public static string GetUser => $"/{s_restVersion}/auth/user";        
        public static string GetBalancesByType(string currency, string typeOfAccount) => $"/{s_restVersion}/auth/account/currency/{currency}/{typeOfAccount}";


        //WS endpoints
        public static string s_latokenWSURL = "wss://api.latoken.com/stomp";
        private static string s_wsVersion = "v1";
        public static char s_stompSeparator = '/';

        //balances
        public static string LAWSAccount(string accountId, out string id)
        {
            id = LAHeaders.ACCOUNT_ID;
            return $"/user/{accountId}/{s_wsVersion}/account";
        }

        //orders
        public static string LAWSOrders(string accountId, out string id)
        {
            id = LAHeaders.ORDER_ID;
            return $"/user/{accountId}/{s_wsVersion}/order";
        }

        //trades
        public static string LAWSMyTrades(string accountId, string baseId, string quoteId, out string id)
        {
            id = $"{LAHeaders.MY_TRADES_ID}{s_stompSeparator}{baseId}{s_stompSeparator}{quoteId}";
            return $"/user/{accountId}/{s_wsVersion}/trade/{baseId}/{quoteId}";
        }

        //orer book
        public static string LAWSOrderBook(string baseId, string quoteId, out string id)
        {
            id = $"{LAHeaders.ORDER_BOOK_ID}{s_stompSeparator}{baseId}{s_stompSeparator}{quoteId}";
            return $"/{s_wsVersion}/book/{baseId}/{quoteId}";
        }
    }
}
