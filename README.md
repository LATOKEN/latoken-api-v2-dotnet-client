# LATOKEN C# Client Library (Early Beta version)

LATOKEN C# Client Library aims to help developers to integrate with the [LATOKEN trading API](https://api.latoken.com/doc/v2/). 

**Please note that the library in Early Beta, and LATOKEN doesn't take any responsibility for the damages occurred while using this code.**
**If you think something is broken or missing, please create an [issue](https://github.com/LATOKEN/latoken-api-v2-dotnet-client/issues).**



## Getting started

Make sure you have installed these Nuget packages:
* Microsoft.Extensions.Logging Version: 5.0.0
* Newtonsoft.Json Version 12.0.3
* Serilog Version: 2.8.0
* Websocket.Client Version: 4.3.35
* MathNet.Numerics Version: 4.15.0


## LARestClient class to interract with the REST API

**Get a list of all accounts**
````C#
    var latokenRestClient =
            new LARestClient(new HttpClient() { BaseAddress = new Uri("https://api.latoken.com") });

    //Generate your public and private API keys on this page https://latoken.com/account/apikeys
    ClientCredentials credentials = new ClientCredentials
    {
        ApiKey = "Your Public API Key",
        ApiSecret = "Your Private API Key"
    };
    latokenRestClient.SetCredentials(credentials);                      

    System.Collections.Generic.List<Account> result = latokenRestClient.GetAccounts().Result;
````

**Placing a limit order**
````C#
    var latokenRestClient =
            new LARestClient(new HttpClient() { BaseAddress = new Uri("https://api.latoken.com") });

    //Generate your public and private API keys on this page https://latoken.com/account/apikeys
    ClientCredentials credentials = new ClientCredentials
    {
        ApiKey = "Your Public API Key",
        ApiSecret = "Your Private API Key"
    };
    latokenRestClient.SetCredentials(credentials);                      

    OrderCommand orderCommand = new OrderCommand
    {
        BaseCurrency = latokenRestClient.GetCurrency("FREE").Result.Id,
        QuoteCurrency = latokenRestClient.GetCurrency("USDT").Result.Id,
        Side = "BUY",
        Condition = "GTC",
        Type = "LIMIT",                
        Quantity = "1.0",
        Price = "1.0",
        ClientOrderId = "C# Client Test",
        Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()

    };
    OrderResponse result = latokenRestClient.PlaceOrder(orderCommand).Result;
````


**Supported REST endpoints (LARestClient class)**

Information about our REST API specification [LATOKEN API v2 docs](https://api.latoken.com/doc/v2/).
| Method | Description | LATOKEN APIv2 Link |
| ----------- | ----------- | ---------|
|`GetPairs`|Request list of all active trading pairs| [getAvailablePairs](https://api.latoken.com/doc/v2/#operation/getAvailablePairs)
|`GetPair`|Get active trading pair| Client library implementation
|`GetCurrencies`|Get a list of active currencies| [getMyOrdersByPair](https://api.latoken.com/doc/v2/#operation/getActiveCurrencies)
|`GetCurrency`|Get a currency by id or tag| [getCurrencies](https://api.latoken.com/doc/v2/#operation/getCurrencies)
|`GetBalances`|Request all account balances for a current user| [getBalancesByUser](https://api.latoken.com/doc/v2/#operation/getBalancesByUser)
|`GetBalanceByType`|Request specific account by currency and type| [getBalancesByCurrency](https://api.latoken.com/doc/v2/#operation/getBalancesByCurrency)
|`GetAvailablePairs`|Request list of all available trading pairs| [getAvailablePairs](https://api.latoken.com/doc/v2/#operation/getAvailablePairs)
|`GetAvailablePair`|Get available trading pair| Client library implementation
|`GetOrders`|Get a list of private orders| [getMyOrders](https://api.latoken.com/doc/v2/#operation/getMyOrders)
|`GetOrdersPair`|Get a list of private orders on a traded pair| [getMyOrdersByPair](https://api.latoken.com/doc/v2/#operation/getMyOrdersByPair)
|`GetOrder`|Get a private order by id| [getOrderById](https://api.latoken.com/doc/v2/#operation/getOrderById)
|`GetAllTrades`|Get public trade history for a given pair| [getTradesByPair](https://api.latoken.com/doc/v2/#operation/getAuthFeeByPair)
|`GetClientTrades`|Get private trade history| [getTradesByTrader](https://api.latoken.com/doc/v2/#operation/getTradesByTrader)
|`GetClientTradesPair`|Request trade history of authorized user for given pair| [getTradesByAssetAndTrader](https://api.latoken.com/doc/v2/#operation/getTradesByAssetAndTrader)
|`PlaceOrder`|Request trade history of authorized user for given pair| [getTradesByAssetAndTrader](https://api.latoken.com/doc/v2/#operation/placeOrder)
|`CancelOrder`|Cancel order by id| [cancelOrder](https://api.latoken.com/doc/v2/#operation/cancelOrder)
|`CancelAllOrders`|Cancel all orders for a given pair| [cancelAllOrders](https://api.latoken.com/doc/v2/#operation/cancelAllOrders)
|`GetOrderBook`|Get order book snapshot for a given pair| [getOrderBook](https://api.latoken.com/doc/v2/#tag/BookController)
|`GetTickers`|Request tickers for all pairs| [getAllTickers](https://api.latoken.com/doc/v2/#operation/getAllTickers)
|`GetTicker`|Request ticker for pair| [getTicker](https://api.latoken.com/doc/v2/#operation/getTicker)
 
 
    
 ## LAWsClient class to interact with the WebSocket API

**Get order book for FREE/USDT pair**
````C#

       var latokenRestClient =
                new LARestClient(new HttpClient() { BaseAddress = new Uri("https://api.latoken.com") });

       ClientCredentials credentials = new ClientCredentials
       {
           Alias = "TestCSharpClient",
           ApiKey = "Your Public API Key",
           ApiSecret = "Your Private API Key"                
       };

            
       latokenRestClient.SetCredentials(credentials);
       LatokenUser result = latokenRestClient.GetUser().Result;

       credentials.UserId = result.Id;

       var wsClient = new LAWsClient(credentials, true);
       wsClient.OnOpened += WSClient_OnOpened;
       wsClient.OnClosed += WSClient_OnClosed;
       wsClient.OnError += WSClient_OnError;

            
       wsClient.Start();
       Thread.Sleep(15000);


       var freeCoin = latokenRestClient.GetCurrency("FREE").Result.Id;
       var usdtCoin = latokenRestClient.GetCurrency("USDT").Result.Id;
                
       wsClient.SubscribeOrderBookEvents(freeCoin, usdtCoin,
       delegate (OrderBookChange orderBookChange, string baseId, string quoteId, DateTime dateTime) {
            Console.WriteLine("Order book change {0}", orderBookChange);
       });


        private void WSClient_OnOpened()
        {
           //TODO: your implementation here
        }


        private void WSClient_OnClosed(WebSocketCloseStatus? status, string description)
        {
            //TODO: your implementation here
        }

        private void WSClient_OnError(string header, string body)
        {
            //TODO: your implementation here
        }

````
**Supported WebSockets endpoints (LAWsClient class)**

Information about our WebSocket API specification [LATOKEN API v2 docs](https://api.latoken.com/doc/ws/).
| Method | Description | LATOKEN APIv2 Link |
| ----------- | ----------- | ---------|
|`SubscribeOrderBookEvents`|Subscribe for order book events for a pair| [Books](https://api.latoken.com/doc/ws/#section/Books)
|`SubscribeOrderEvents`|Subscribe for private order events| [Orders](https://api.latoken.com/doc/ws/#section/Orders)
|`SubscribeMyTradeEvents`|Subscribe for private trade events| [Trades](https://api.latoken.com/doc/ws/#section/Trades)
|`SubscribeBalanceEvents`|Subscribe for balance on your accounts events| [Accounts](https://api.latoken.com/doc/ws/#section/Accounts)




## Do you have any API questions? 
[Use Telegram](https://telegram.org/), and talk to us in our channel: https://t.me/latoken_api
