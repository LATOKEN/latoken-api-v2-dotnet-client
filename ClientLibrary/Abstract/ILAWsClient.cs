using Latoken_CSharp_Client_Library.Dto.WS;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Latoken_CSharp_Client_Library.Abstract
{
    public interface ILAWsClient
    {
        event Action OnOpened;
        event Action<WebSocketCloseStatus?, string> OnClosed;
        event Action<string, string> OnError;

        bool IsStompConnected();
        void Start();
        void Dispose();

        void SubscribeBalanceEvents(Action<List<Balance>, DateTime> balancesCallback);
        void SubscribeOrderEvents(Action<List<LatokenLatoken_CSharp_Client_Library.Order>, DateTime> ordersCallback);
        void SubscribeMyTradeEvents(string baseId, string quoteId, Action<List<LatokenLatoken_CSharp_Client_Library.Dto.WS.MyTrade>, DateTime> tradesCallback);
        void SubscribeOrderBookEvents(string baseId, string quoteId, Action<OrderBookChange, string, string, DateTime> orderbooksCallback);
    }
}
