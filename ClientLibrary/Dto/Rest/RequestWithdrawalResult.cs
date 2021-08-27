using Newtonsoft.Json;

namespace Latoken_CSharp_Client_Library.Dto.Rest
{
    public class RequestWithdrawalResult
    {
        [JsonProperty(PropertyName = "withdrawalId")]
        public string withdrawalId { get; set; }

        [JsonProperty(PropertyName = "codeRequired")]
        public string codeRequired { get; set; }

        [JsonProperty(PropertyName = "transaction")]
        public Transaction Transaction { get; set; }
    }
}
