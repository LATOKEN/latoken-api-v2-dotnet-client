namespace Latoken_CSharp_Client_Library.Settings.ConfigClient
{
    public static class ConfigApi
    {
        private const string Tag = "api/cf/";

        public static class Options
        {
            private static readonly string RootPath = Tag + nameof(Options);

            public static string GetApiOption(string account, KeyPermission permission, TargetExchange exchange) 
                => $"{RootPath}/GetApiOption?account={account}&keyType={permission}&exchange={exchange}";
        }
    }
}
