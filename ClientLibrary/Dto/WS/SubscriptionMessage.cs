using Newtonsoft.Json;

namespace Latoken.Api.Client.Library.Dto.WS
{
    public class SubscriptionMessage<T>
    {
        [JsonProperty(PropertyName = "payload")]
        public T Payload { get; set; }

        [JsonProperty(PropertyName = "nonce")]
        public int Nonce { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }
    }
}
