using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreatedBookingEvent
    {
        int BookingId { get; }
        int FlightDetailsId { get; }
        int CarDetailsId { get; }
        DateTime CreatedDate { get; }
        Guid CorrelationId { get; }
    }
}