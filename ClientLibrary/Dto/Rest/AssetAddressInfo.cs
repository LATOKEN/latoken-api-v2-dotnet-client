using Newtonsoft.Json;
using System.Collections.Generic;

namespace Latoken_CSharp_Client_Library.Dto.Rest
{
    public class AssetAddressInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tag")]
        public string Asset { get; set; }

        [JsonProperty("bindings")]
        public List<AssetAddressBinding> Bindings { get; set; }

    }
}
