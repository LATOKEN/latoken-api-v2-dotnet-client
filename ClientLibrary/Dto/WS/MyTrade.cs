using Newtonsoft.Json;

namespace LatokenLatoken_CSharp_Client_Library.Dto.WS
{
    public class MyTrade : Latoken_CSharp_Client_Library.Trade
    {
        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }
        [JsonProperty(PropertyName = "makerBuyer")]
        public bool MakerBuyer { get; set; }
    }
}
