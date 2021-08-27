using System.Net.Http;
using System.Net.Http.Headers;

namespace Latoken_CSharp_Client_Library
{
    public class LatokenHttpClient : HttpClient
    {
        public LatokenHttpClient(string botType, string botId, string environment = "local", HttpMessageHandler handler = default)
            : base(handler ?? new HttpClientHandler())
        {
            DefaultRequestHeaders.UserAgent.Clear();
            DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Latoken","MarketMaking"));
            DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(botType, botId));
            DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue($"({environment})"));
        }
    }
}