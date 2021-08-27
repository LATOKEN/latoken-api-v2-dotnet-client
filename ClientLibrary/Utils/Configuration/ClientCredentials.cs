namespace Latoken_CSharp_Client_Library.Utils.Configuration
{
    public class ClientCredentials
    {
        public ClientCredentials()
        {
        }
        //Set any name to identify your client.
        //For example, YourCompanyName C# Client
        public string Alias { get; set; }
        //Public API key
        public string ApiKey { get; set; }
        //Private API key
        public string ApiSecret { get; set; }

        //For websockets only: User UUID as string,
        //you can call REST GetUser() method, and fetch the id field
        public string UserId { get; set; }
    }
}
