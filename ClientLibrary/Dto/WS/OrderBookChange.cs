using Newtonsoft.Json;
using System.Collections.Generic;

namespace Latoken.Api.Client.Library.Dto.WS
{
    public class OrderBookChange
    {
        [JsonProperty(PropertyName = "ask")]
        public List<OrderChange> Ask { get; set; }
        [JsonProperty(PropertyName = "bid")]
        public List<OrderChange> Bid { get; set; }
    }
}
