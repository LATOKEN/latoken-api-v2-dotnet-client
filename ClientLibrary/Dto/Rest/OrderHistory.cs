using Newtonsoft.Json;
using System.Collections.Generic;

namespace Latoken_CSharp_Client_Library
{
    public class OrderHistory
    {
        [JsonProperty(PropertyName = "hasNext")]
        public bool HasNext { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "first")]
        public bool First { get; set; }

        [JsonProperty(PropertyName = "hasContent")]
        public bool HasContent { get; set; }

        [JsonProperty(PropertyName = "content")]
        public List<LatokenLatoken_CSharp_Client_Library.Order> Orders { get; set; }
    }
}
