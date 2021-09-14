namespace Latoken.Api.Client.Library.Utils.Configuration
{
    public class ClientCredentials
    {
        public ClientCredentials()
        {
        }
        /// <summary>
        //  Set any name to identify your client.
        //  For example, YourCompanyName C# Client
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        //  Public API key
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        //  Private API key
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        //  For websockets only: User UUID as string,
        //  you can call REST GetUser() method, and fetch the id field
        /// </summary>
        public string UserId { get; set; }
    }
}
