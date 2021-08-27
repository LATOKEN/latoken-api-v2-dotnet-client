using System;
using System.Collections.Generic;
using System.Text;
using Latoken_CSharp_Client_Library.Constants;

namespace Latoken_CSharp_Client_Library.Commands.Stomp
{
    public class StompMessage
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompMessage" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        public StompMessage(string command)
            : this(command, string.Empty)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompMessage" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "body">The body.</param>
        public StompMessage(string command, string body)
            : this(command, body, new Dictionary<string, string>())
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompMessage" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "body">The body.</param>
        /// <param name = "headers">The headers.</param>
        internal StompMessage(string command, string body, Dictionary<string, string> headers)
        {
            Command = command;
            Body = body;
            _headers = headers;

            if (body.Length > 0)
                this["content-length"] = body.Length.ToString();
        }

        public Dictionary<string, string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// Gets or sets the specified header attribute.
        /// </summary>
        public string this[string header]
        {
            get { return _headers.ContainsKey(header) ? _headers[header] : string.Empty; }
            set { _headers[header] = value; }
        }

        public static StompMessage CreateConnectMessage(string version, string heartbeat)
        {
            var msg = new StompMessage(StompFrame.CONNECT);
            msg[LAHeaders.VERSION_HEADER] = version;
            msg[LAHeaders.HEARTBEAT_HEADER] = heartbeat;
            return msg;
        }

        public static StompMessage CreateSignedConnectMessage(string version, string heartbeat, string apiKey, string apiSecret, string hashAlgorithm = null)
        {
            string signData = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString();
            var msg = new StompMessage(StompFrame.CONNECT);
            msg[LAHeaders.VERSION_HEADER] = version;
            msg[LAHeaders.HEARTBEAT_HEADER] = heartbeat;
            msg[LAHeaders.LA_APIKEY] = apiKey;
            msg[LAHeaders.LA_DIGEST] = hashAlgorithm ?? LAHeaders.HASH_ALGO;
            msg[LAHeaders.LA_SIGDATA] = signData;
            msg[LAHeaders.LA_SIGNATURE] = SignatureService.CreateSignature(apiSecret, signData);
            return msg;
        }

        public static StompMessage CreateSubsribeMessage(string endpoint, string id)
        {
            var msg = new StompMessage(StompFrame.SUBSCRIBE);
            msg[LAHeaders.DESTINATION_HEADER] = endpoint;
            msg[LAHeaders.ID_HEADER] = id;
            return msg;
        }

        public static StompMessage CreateUnsubsribeMessage(string id)
        {
            var msg = new StompMessage(StompFrame.UNSUBSCRIBE);
            msg[LAHeaders.ID_HEADER] = id;
            return msg;
        }

        public static StompMessage CreateDisconnectMessage(string receiptId)
        {
            var msg = new StompMessage(StompFrame.DISCONNECT);
            msg[LAHeaders.RECEIPT_HEADER] = receiptId;
            return msg;
        }
    }
}
