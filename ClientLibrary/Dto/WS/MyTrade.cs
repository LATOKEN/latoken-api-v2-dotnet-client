using Newtonsoft.Json;

namespace Latoken.Api.Client.Library.Dto.WS
{
    public class MyTrade : Trade
    {
        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }
        [JsonProperty(PropertyName = "makerBuyer")]
        public bool MakerBuyer { get; set; }
    }
}
