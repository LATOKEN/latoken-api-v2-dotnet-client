using Newtonsoft.Json;
using System.Collections.Generic;

namespace Latoken_CSharp_Client_Library.Dto.Rest
{
    public class LatokenUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "authType")]
        public string AuthType { get; set; }

        [JsonProperty(PropertyName = "authorities")]
        public List<string> Authorities { get; set; }


    }
}
