using Latoken.Api.Client.Library.Types;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Latoken.Api.Client.Library.Commands
{
    public class OrderCommand
    {
        public OrderCommand()
        {
        }

        public OrderCommand(string baseCurrency, 
            string quoteCurrency, 
            OrderSide side, 
            OrderCondition condition, 
            OrderType type, 
            string clientOrderId, 
            decimal price, 
            decimal quantity)
        {
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
            Side = side.ToString();
            Condition = condition.ToString();
            Type = type.ToString();
            ClientOrderId = clientOrderId;
            Price = price.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
            Quantity = quantity.ToString(CultureInfo.InvariantCulture).Replace(",", ".");           
        }

        [JsonProperty(PropertyName = "baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonProperty(PropertyName = "quoteCurrency")]
        public string QuoteCurrency { get; set; }

        [JsonProperty(PropertyName = "side")]
        public string Side { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "clientOrderId")]
        public string ClientOrderId { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public string Quantity { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; } = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
}
