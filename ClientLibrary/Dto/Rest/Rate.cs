using Newtonsoft.Json;

namespace Latoken_CSharp_Client_Library
{
    public class Rate
    {
        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }
    }
}