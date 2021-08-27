using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Latoken_CSharp_Client_Library.Dto.WS
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
