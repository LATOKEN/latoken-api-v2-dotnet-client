using Newtonsoft.Json;
using System.Collections.Generic;

namespace Latoken_CSharp_Client_Library.Dto.WS
{
    public class OrderBookChange
    {
        [JsonProperty(PropertyName = "ask")]
        public List<OrderChange> Ask { get; set; }
        [JsonProperty(PropertyName = "bid")]
        public List<OrderChange> Bid { get; set; }
    }
}
