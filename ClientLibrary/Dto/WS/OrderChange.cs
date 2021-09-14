using Newtonsoft.Json;

namespace Latoken.Api.Client.Library.Dto.WS
{
    public class OrderChange
    {
        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }
        [JsonProperty(PropertyName = "quantityChange")]
        public string QuantityChange { get; set; }
        [JsonProperty(PropertyName = "costChange")]
        public string CostChange { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public string Quantity { get; set; }    
        [JsonProperty(PropertyName = "cost")]
        public string Cost { get; set; }
    }
}
