using Newtonsoft.Json;

namespace Latoken_CSharp_Client_Library
{
    public class Ticker
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonProperty(PropertyName = "quoteCurrency")]
        public string QuoteCurrency { get; set; }

        [JsonProperty(PropertyName = "volume24h")]
        public decimal Volume24h { get; set; }

        [JsonProperty(PropertyName = "volume7d")]
        public decimal Volume7d { get; set; }

        [JsonProperty(PropertyName = "change24h")]
        public decimal Change24h { get; set; }

        [JsonProperty(PropertyName = "change7d")]
        public decimal Change7d { get; set; }

        [JsonProperty(PropertyName = "lastPrice")]
        public decimal LastPrice { get; set; }
    }
}
