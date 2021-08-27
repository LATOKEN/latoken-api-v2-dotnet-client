using System.Collections.Generic;
using System.Net;

namespace Latoken_CSharp_Client_Library
{
    public enum ApiResponseCode
    {
        Success = 1,
        RateLimiting,
        ClientTimeout,
        ServiceUnavailable,
        UnavailableForLegalReasons,
        InsufficientFundsBuy,
        InsufficientFundsSell,
        TooManyActiveOrders,
        ApiKeyWasRevoked,
        PairStatusInactive,
        QuantityIsLowerThanNeeded,
        InvalidStateTransition,
        DeadlineExceeded,
        CurrencyNotFound,
        Conflict,
        BadGateway,
        BadTicks,
        InsufficientPrivileges,
        UnknownBadRequest,
        UnknownNotFound,
        UnknownInternalServerError,
        Unknown
    }

    public static class ApiResponseCodeExtensions
    {
        private static readonly Dictionary<ApiResponseCode, HttpStatusCode> CodesMap =
            new Dictionary<ApiResponseCode, HttpStatusCode>
            {
                [ApiResponseCode.RateLimiting] = HttpStatusCode.TooManyRequests,
                [ApiResponseCode.ServiceUnavailable] = HttpStatusCode.ServiceUnavailable,
                [ApiResponseCode.InsufficientFundsSell] = HttpStatusCode.BadRequest,
                [ApiResponseCode.InsufficientFundsBuy] = HttpStatusCode.BadRequest,
                [ApiResponseCode.TooManyActiveOrders] = HttpStatusCode.BadRequest,
                [ApiResponseCode.ApiKeyWasRevoked] = HttpStatusCode.BadRequest,
                [ApiResponseCode.PairStatusInactive] = HttpStatusCode.BadRequest,
                [ApiResponseCode.QuantityIsLowerThanNeeded] = HttpStatusCode.BadRequest,
                [ApiResponseCode.DeadlineExceeded] = HttpStatusCode.GatewayTimeout,
                [ApiResponseCode.Conflict] = HttpStatusCode.Conflict,
                [ApiResponseCode.BadGateway] = HttpStatusCode.BadGateway,
                [ApiResponseCode.UnknownBadRequest] = HttpStatusCode.BadRequest,
                [ApiResponseCode.UnavailableForLegalReasons] = HttpStatusCode.UnavailableForLegalReasons,
                [ApiResponseCode.CurrencyNotFound] = HttpStatusCode.NotFound,
                [ApiResponseCode.UnknownNotFound] = HttpStatusCode.NotFound,
                [ApiResponseCode.UnknownInternalServerError] = HttpStatusCode.InternalServerError,
                [ApiResponseCode.InvalidStateTransition] = HttpStatusCode.BadRequest,
                [ApiResponseCode.BadTicks] = HttpStatusCode.BadRequest,
                [ApiResponseCode.InsufficientPrivileges] = HttpStatusCode.BadRequest,
            };
        
        public static string GetStatusCode(this ApiResponseCode code)
        {
            return CodesMap.ContainsKey(code) ? CodesMap[code].ToString() : "Unknown";
        }
    }
}