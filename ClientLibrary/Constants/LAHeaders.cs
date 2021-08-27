namespace Latoken_CSharp_Client_Library.Constants
{
    public class LAHeaders
    {
        public const string LA_APIKEY = "X-LA-APIKEY";
        public const string LA_SIGNATURE = "X-LA-SIGNATURE";
        public const string LA_DIGEST = "X-LA-DIGEST";
        public const string LA_SIGDATA = "X-LA-SIGDATA";
        public const string MediaType = "application/json";
        public const string HASH_ALGO = "HMAC_SHA256";

        //stomp
        public const string HEARTBEAT_HEADER = "heart-beat";
        public const string VERSION_HEADER = "accept-version";
        public const string DESTINATION_HEADER = "destination";
        public const string ID_HEADER = "id";
        public const string RECEIPT_HEADER = "receipt";
        public const string SUBSCRIPTION_HEADER = "subscription";
        public const string MESSAGE_HEADER = "message";
        public const string ACCOUNT_ID = "balances";
        public const string ORDER_ID = "orders";
        public const string MY_TRADES_ID = "mytrades";
        public const string ORDER_BOOK_ID = "orderbook";

        //errors
        public const string NONCE_MISMATCH_HEADER = "Subscription message nonce has mismatch with an expected value! Unsubscribing..";
        public const string WS_ERROR_HEADER = "Latoken web socket error!";
        public const string STOMP_NO_HEARTBEAT = "No heartbeat messages within given period!";
        public const string WS_NOT_ALIVE = "Latoken web socket connection is not alive!";
        public const string STOMP_NOT_ALIVE = "Latoken stomp connection is not alive!";
    }
}
