namespace Latoken_CSharp_Client_Library.Constants
{
    public static class ApiPath
    {
        public static class HttpMethod
        {
            public static string Get = "GET";
            public static string Post = "POST";
        } 

        private static string v = "v2";

        // BookController
        public static string GetOrderBook(string baseCurrency, string quoteCurrency, int limit) => $"/{v}/book/{baseCurrency}/{quoteCurrency}?limit={limit}";

        // PairController
        public static string GetPairs = $"/{v}/pair";

        public static string GetAvailablePairs = $"/{v}/pair/available";

        // CurrencyController 
        public static string GetCurrency(string currency) => $"/{v}/currency/{currency}";
        public static string GetCurrencies = $"/{v}/currency";

        // TickerController
        public static string GetTickers = $"/{v}/ticker";
        public static string GetTicker(string baseCurrency, string quoteCurrency) => $"/{v}/ticker/{baseCurrency}/{quoteCurrency}";
        public static string GetRate(string baseCurrency, string quoteCurrency) => $"/{v}/rate/{baseCurrency}/{quoteCurrency}";

        // TradeController Public
        public static string GetAllTrades(string baseCurrency, string quoteCurrency) => 
            $"{v}/trade/history/{baseCurrency}/{quoteCurrency}";

        // TradeController Auth
        public static string GetClientTrades(int page, int size) => $"/{v}/auth/trade?page={page}&size={size}";
        public static string GetClientTradesPair(string baseCurrency, string quoteCurrency, int page, int size) => 
            $"/{v}/auth/trade/pair/{baseCurrency}/{quoteCurrency}?page={page}&size={size}";

        // OrderController Auth
        public static string GetOrders(int size) => $"/{v}/auth/order?limit={size}";
        public static string GetOrdersPair(string baseCurrency, string quoteCurrency, int page, int size) => 
            $"{v}/auth/order/pair/{baseCurrency}/{quoteCurrency}/active?size={size}&page={page}";

        public static string PlaceOrder = $"{v}/auth/order/place";
        public static string CancelOrder = $"{v}/auth/order/cancel";
        public static string GetOrder(string orderId) => $"/{v}/auth/order/getOrder/{orderId}";
        public static string TransferByUserId = $"/{v}/auth/transfer/id";
        public static string SpotWithdraw = "/v2/auth/transfer/spot/withdraw";
        public static string CancelAllOrders(string baseCurrency, string quoteCurrency) => $"{v}/auth/order/cancelAll/{baseCurrency}/{quoteCurrency}";

        // AccountController Auth
        public static string GetBalances(bool zeros) => $"/{v}/auth/account?zeros={zeros}";
        public static string GetUser => $"/{v}/auth/user";        
        public static string GetBalancesByType(string currency, string typeOfAccount) => $"/{v}/auth/account/currency/{currency}/{typeOfAccount}";

        public static string GetKlines(string symbol, int intervalMin, long @from, long to) => $"{v}/tradingview/history?symbol={symbol}&resolution={intervalMin}&from={from}&to={to}";


        //WS endpoints
        public static string LAWSApiUrl = "wss://api.latoken.com/stomp";
        private static string v_ws = "v1";
        public static char STOMP_SEPARATOR = '/';

        //balances
        public static string LAWSAccount(string accountId, out string id)
        {
            id = LAHeaders.ACCOUNT_ID;
            return $"/user/{accountId}/{v_ws}/account";
        }

        //orders
        public static string LAWSOrders(string accountId, out string id)
        {
            id = LAHeaders.ORDER_ID;
            return $"/user/{accountId}/{v_ws}/order";
        }

        //trades
        public static string LAWSMyTrades(string accountId, string baseId, string quoteId, out string id)
        {
            id = $"{LAHeaders.MY_TRADES_ID}{STOMP_SEPARATOR}{baseId}{STOMP_SEPARATOR}{quoteId}";
            return $"/user/{accountId}/{v_ws}/trade/{baseId}/{quoteId}";
        }

        //orer book
        public static string LAWSOrderBook(string baseId, string quoteId, out string id)
        {
            id = $"{LAHeaders.ORDER_BOOK_ID}{STOMP_SEPARATOR}{baseId}{STOMP_SEPARATOR}{quoteId}";
            return $"/{v_ws}/book/{baseId}/{quoteId}";
        }
    }
}
