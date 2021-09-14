using Newtonsoft.Json;

namespace Latoken.Api.Client.Library.Dto.Rest
{
    public class AssetAddressBinding
    {
        [JsonProperty("minAmount")]
        public decimal MinAmount { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("providerName")]
        public string ProviderName { get; set; }

        [JsonProperty("id")]
        public string ProviderId { get; set; }
    }
}
