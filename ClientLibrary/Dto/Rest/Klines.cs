using System.Collections.Generic;
using Newtonsoft.Json;

namespace Latoken_CSharp_Client_Library
{
    public class Klines
    {
        [JsonProperty(PropertyName = "c")]
        public List<decimal> ClosePrices { get; set; }

        [JsonProperty(PropertyName = "o")]
        public List<decimal> OpenPrices { get; set; }

        [JsonProperty(PropertyName = "s")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "t")]
        public List<long> Timestamps { get; set; }

        [JsonProperty(PropertyName = "h")]
        public List<decimal> HighPrices { get; set; }

        [JsonProperty(PropertyName = "l")]
        public List<decimal> LowPrices { get; set; }

        [JsonProperty(PropertyName = "v")]
        public List<decimal> Volumes { get; set; }
    }
}