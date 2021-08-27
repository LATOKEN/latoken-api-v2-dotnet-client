using Newtonsoft.Json;
using System.Collections.Generic;

namespace Latoken_CSharp_Client_Library
{
    public class OrderBook
    {
        [JsonProperty(PropertyName = "ask")]
        public List<PriceLevel> Ask { get; set; }

        [JsonProperty(PropertyName = "bid")]
        public List<PriceLevel> Bid { get; set; }

        [JsonProperty(PropertyName = "totalAsk")]
        public decimal TotalAsk { get; set; }

        [JsonProperty(PropertyName = "totalBid")]
        public decimal TotalBid { get; set; }
    }

    public class PriceLevel
    {
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public decimal Cost { get; set; }

        [JsonProperty(PropertyName = "accumulated")]
        public decimal Accumulated { get; set; }
    }
}
