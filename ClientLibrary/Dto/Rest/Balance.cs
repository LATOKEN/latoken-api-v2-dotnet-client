using Newtonsoft.Json;
namespace Latoken_CSharp_Client_Library
{
    public class Balance
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string CurrencyId { get; set; }

        [JsonProperty(PropertyName = "available")]
        public decimal Available { get; set; }

        [JsonProperty(PropertyName = "blocked")]
        public decimal Blocked { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public long Timestampde { get; set; }
    }
}


