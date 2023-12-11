using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateCarBookingEvent
    {
        int BookingId { get; }
        int FlightDetailsId { get; }
        Guid CorrelationId { get; }
        int CarDetailsId { get; }
        DateTime CreatedDate { get; }
    }
}