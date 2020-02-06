using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PushNotification.IOS.Model
{
    class APNsResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccess { get; set; }
        public APNsError Error { get; set; }
    }

    public class APNsError
    {
        public ReasonEnum Reason { get; set; }
        public long? Timestamp { get; set; }
    }

    public enum ReasonEnum
    {
        BadCollapseId,
        BadDeviceToken,
        BadExpirationDate,
        BadMessageId,
        BadPriority,
        BadTopic,
        DeviceTokenNotForTopic,
        DuplicateHeaders,
        IdleTimeout,
        MissingDeviceToken,
        MissingTopic,
        PayloadEmpty,
        TopicDisallowed,
        BadCertificate,
        BadCertificateEnvironment,
        ExpiredProviderToken,
        Forbidden,
        InvalidProviderToken,
        MissingProviderToken,
        BadPath,
        MethodNotAllowed,
        Unregistered,
        PayloadTooLarge,
        TooManyProviderTokenUpdates,
        TooManyRequests,
        InternalServerError,
        ServiceUnavailable,
        Shutdown,
    }
}
