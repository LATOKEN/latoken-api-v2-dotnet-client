using Newtonsoft.Json;

namespace Latoken.Api.Client.Library
{
    public class Currency
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public string Logo { get; set; }

        [JsonProperty(PropertyName = "decimals")]
        public int Decimals { get; set; }

        [JsonProperty(PropertyName = "created")]
        public long Created { get; set; }
    }
}
