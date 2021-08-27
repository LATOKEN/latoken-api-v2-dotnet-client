using Newtonsoft.Json;

namespace Latoken_CSharp_Client_Library
{
    public class Pair
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonProperty(PropertyName = "quoteCurrency")]
        public string QuoteCurrency { get; set; }

        [JsonProperty(PropertyName = "priceTick")]
        public decimal PriceTick { get; set; }

        [JsonProperty(PropertyName = "priceDecimals")]
        public int PriceDecimals { get; set; }

        [JsonProperty(PropertyName = "quantityTick")]
        public decimal QuantityTick { get; set; }

        [JsonProperty(PropertyName = "quantityDecimals")]
        public int QuantityDecimals { get; set; }

        [JsonProperty(PropertyName = "costDisplayDecimals")]
        public int CostDisplayDecimals { get; set; }

        [JsonProperty(PropertyName = "created")]
        public long Created { get; set; }

        [JsonProperty(PropertyName = "minOrderQuantity", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MinOrderQuantity { get; set; }

        [JsonProperty(PropertyName = "maxOrderCostUsd", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MaxOrderCostUsd { get; set; }

        [JsonProperty(PropertyName = "minOrderCostUsd", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MinOrderCostUsd { get; set; }

    }
}
