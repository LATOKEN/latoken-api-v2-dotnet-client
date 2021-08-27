using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Latoken_CSharp_Client_Library
{
    public class ApiErrorParser
    {
        private static readonly Regex InsufficientFundsRegex = new Regex("\"side\"\\s*:\\s*\"(?'side'.*?)\"", RegexOptions.IgnoreCase);
        
        public async Task<ApiResponseCode> GetErrorCode(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (response.IsSuccessStatusCode)
                return ApiResponseCode.Success;
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return await ParseBadRequest(response);
                case HttpStatusCode.InternalServerError:
                    return await ParseError(response, "SERVICE_UNAVAILABLE", 
                        ApiResponseCode.ServiceUnavailable, ApiResponseCode.UnknownInternalServerError);
                case HttpStatusCode.GatewayTimeout:
                    return await ParseError(response, "DEADLINE_EXCEEDED", ApiResponseCode.DeadlineExceeded, ApiResponseCode.Unknown);
                case HttpStatusCode.NotFound:
                    return await ParseError(response, "Unable to resolve currency by tag",  
                        ApiResponseCode.CurrencyNotFound, ApiResponseCode.UnknownNotFound);
                case HttpStatusCode.Conflict:
                    return ApiResponseCode.Conflict;
                case HttpStatusCode.TooManyRequests:
                    return ApiResponseCode.RateLimiting;
                case HttpStatusCode.BadGateway:
                    return ApiResponseCode.BadGateway;
                case HttpStatusCode.ServiceUnavailable:
                    return ApiResponseCode.ServiceUnavailable;
                case HttpStatusCode.UnavailableForLegalReasons:
                    return ApiResponseCode.UnavailableForLegalReasons;
                default:
                    return ApiResponseCode.Unknown;
            }
        }

        private static async Task<ApiResponseCode> ParseBadRequest(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (content.Contains("INSUFFICIENT_FUNDS"))
                return await ParseInsufficientFunds(response.RequestMessage);

            if (content.Contains("You already have too much active orders in this pair"))
                return ApiResponseCode.TooManyActiveOrders;

            if (content.Contains("PAIR_STATUS_INACTIVE"))
                return ApiResponseCode.PairStatusInactive;

            if (content.Contains("is lower than minimal allowed quantity"))
                return ApiResponseCode.QuantityIsLowerThanNeeded;

            if (content.Contains("Invalid state transition"))
                return ApiResponseCode.InvalidStateTransition;

            if (content.Contains("BAD_TICKS"))
                return ApiResponseCode.BadTicks;
            
            if (content.Contains("Order placement rejected: insufficient privileges"))
                return ApiResponseCode.InsufficientPrivileges;
            
            return content.Contains("The API key was revoked")
                ? ApiResponseCode.ApiKeyWasRevoked
                : ApiResponseCode.UnknownBadRequest;
        }

        private static async Task<ApiResponseCode> ParseInsufficientFunds(HttpRequestMessage request)
        {
            var content = await request.Content.ReadAsStringAsync();
            var match = InsufficientFundsRegex.Match(content);
            var side = match.Groups["side"].Value;
            return side.ToUpper() == "BID" ? ApiResponseCode.InsufficientFundsBuy : ApiResponseCode.InsufficientFundsSell;
        }
        
        private static async Task<ApiResponseCode> ParseError(HttpResponseMessage response, string message, ApiResponseCode positive, ApiResponseCode negative)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content.Contains(message) ? positive : negative;
        }
    }
}