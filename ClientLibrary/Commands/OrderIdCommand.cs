using Newtonsoft.Json;

namespace Latoken_CSharp_Client_Library.Commands
{
    public class OrderIdCommand
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
