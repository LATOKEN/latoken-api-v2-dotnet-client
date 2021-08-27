using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Latoken_CSharp_Client_Library
{
    public class CancelAllOrdersResponce
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public Dictionary<string, string> Errors { get; set; }
    }
}
