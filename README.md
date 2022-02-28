# LATOKEN C# Client Library (Early Beta version)

LATOKEN C# Client Library aims to help developers to integrate with the [LATOKEN trading API](https://api.latoken.com/doc/v2/). 
The WebSocket connection is based on the STOMP protocol. More details can be found [here](https://stomp.github.io/).

**Please note that the library in Early Beta, and LATOKEN doesn't take any responsibility for the damages occurred while using this code.**
**If you think something is broken or missing, please create an [issue](https://github.com/LATOKEN/latoken-api-v2-dotnet-client/issues).**



## Getting started
You can install this as a Nuget package: Latoken.Api.Client.Library 1.0.0

Alternatively, you can use the source code, but make sure you have installed these Nuget packages:
* Microsoft.Extensions.Logging Version: 5.0.0
* Newtonsoft.Json Version 12.0.3
* Websocket.Client Version: 4.3.35

## Important
Note that this library is not thread-safe. Don't attempt to send orders from multiple threads etc.

## LARestClient class to interract with the REST API

**Get a list of all account balances**
````C#
    //Generate your public and private API keys on this page https://latoken.com/account/apikeys
    ClientCredentials credentials = new ClientCredentials
    {
        ApiKey = "Your Public API Key",
        ApiSecret = "Your Private API Key"
    };

    var latokenRestClient =
            new LARestClient(credentials, new HttpClient() { BaseAddress = new Uri("https://api.latoken.com") });

    

    List<Balance> balances = await latokenRestClient.GetBalances();

````

**Placing a limit order**
````C#
    //Generate your public and private API keys on this page https://latoken.com/account/apikeys
    ClientCredentials credentials = new ClientCredentials
    {
        ApiKey = "Your Public API Key",
        ApiSecret = "Your Private API Key"
    };

    var latokenRestClient =
            new LARestClient(credentials, new HttpClient() { BaseAddress = new Uri("https://api.latoken.com") });

   
    //Getting UUID strings representing IDs of the corresponding currencies/assets
    //You might want to cache this, since this is static data, and  you don't need to fetch it each time
    var baseCurrency = (await latokenRestClient.GetCurrency("FREE")).Id;
    var quoteCurrency = (await latokenRestClient.GetCurrency("USDT")).Id;

    //Consult https://api.latoken.com/doc/v2/#operation/placeOrder for possible field values
    //Also, follow price and qty format as described on this page, use comma only to separate the decimal part
    OrderCommand orderCommand = new OrderCommand
    {
        BaseCurrency = baseCurrency,
        QuoteCurrency = quoteCurrency,
        BaseCurrency = freeCoin,
        QuoteCurrency = usdtCoin,
        Side = "SELL",
        Condition = "GTC",
        Type = "LIMIT",                
        Quantity = "1.0",
        Price = "1.0",
        ClientOrderId = Guid.NewGuid().ToString(),
        Timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds

    };
    OrderResponse result = await latokenRestClient.PlaceOrder(orderCommand);
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

        ClientCredentials restCredentials = new ClientCredentials
       {
           Alias = "TestCSharpClient",
           ApiKey = "Your Public API Key",
           ApiSecret = "Your Private API Key"                
       };
       var latokenRestClient =
                new LARestClient(restCredentials, new HttpClient() { BaseAddress = new Uri("https://api.latoken.com") });

            
       LatokenUser getUserResult = await latokenRestClient.GetUser();

       ClientCredentials wsCredentials = new ClientCredentials
       {
           Alias = "TestCSharpClient",
           ApiKey = "Your Public API Key",
           ApiSecret = "Your Private API Key",
           UserId = getUserResult.Id
       };

       var wsClient = new LAWsClient(wsCredentials, true);
       wsClient.OnOpened += WSClient_OnOpened;
       wsClient.OnClosed += WSClient_OnClosed;
       wsClient.OnError += WSClient_OnError;

            
       wsClient.Start();
       await Task.Delay(15000);


       //Getting UUID strings representing IDs of the corresponding currencies/assets
       //You might want to cache this, since this is static data, and  you don't need to fetch it each time
        var freeCoin = (await latokenRestClient.GetCurrency("FREE")).Id;
        var usdtCoin = (await latokenRestClient.GetCurrency("USDT")).Id;
                
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