using Newtonsoft.Json;
using System.Collections.Generic;

namespace Latoken_CSharp_Client_Library
{
    public class OrderResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public Dictionary<string, string> Errors { get; set; }
        
        public ApiResponseCode ErrorCode { get; set; }
    }
}
