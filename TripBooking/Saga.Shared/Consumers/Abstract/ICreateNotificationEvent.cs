using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateNotificationEvent
    {
        Guid CorrelationId { get; }
        bool IsSuccessful { get; }
        string Message { get; }
        int BookingId { get; }
        int FlightDetailsId { get; }
        int CarDetailsId { get; }
    }
}