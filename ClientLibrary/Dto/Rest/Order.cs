using Newtonsoft.Json;

namespace Latoken.Api.Client.Library {
    public class Order
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "side")]
        public string Side { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonProperty(PropertyName = "quoteCurrency")]
        public string QuoteCurrency { get; set; }

        [JsonProperty(PropertyName = "clientOrderId")]
        public string ClientOrderId { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public decimal? Cost { get; set; }

        [JsonProperty(PropertyName = "filled")]
        public decimal Filled { get; set; }

        [JsonProperty(PropertyName = "trader")]
        public string Trader { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }
    }
}
