using Newtonsoft.Json;

namespace Latoken.Api.Client.Library.Commands
{
    public class OrderIdCommand
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
