using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Latoken_CSharp_Client_Library
{
    public class TradeHistory
    {
        [JsonProperty(PropertyName = "first")]
        public bool First { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "hasContent")]
        public bool HasContent { get; set; }

        [JsonProperty(PropertyName = "hasNext")]
        public bool HasNext { get; set; }

        [JsonProperty(PropertyName = "content")]
        public List<Trade> Trades { get; set; }
    }
}
