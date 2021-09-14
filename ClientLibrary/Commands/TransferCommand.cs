using Newtonsoft.Json;

namespace Latoken.Api.Client.Library.LA.Commands
{
    public class TransferCommand
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "recipient")]
        public string Recipient { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class SpotTransferCommand
    {
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }       

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
