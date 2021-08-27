namespace Latoken_CSharp_Client_Library
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string TransferringFunds { get; set; }
        public decimal UsdValue { get; set; }
        public string RejectReason { get; set; }
        public long Timestamp { get; set; }
        public string Direction { get; set; }
        public string Method { get; set; }
        public string Recipient { get; set; }
        public string Currency { get; set; }

    }
}
