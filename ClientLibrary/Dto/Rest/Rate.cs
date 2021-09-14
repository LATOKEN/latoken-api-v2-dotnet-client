using Newtonsoft.Json;

namespace Latoken.Api.Client.Library
{
    public class Rate
    {
        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }
    }
}