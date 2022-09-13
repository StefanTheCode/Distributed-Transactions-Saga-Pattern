using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateFlightBookingEvent
    {
        int BookingId { get; }
        int FlightId { get; }
        DateTime CreatedDate { get; }
        Guid CorrelationId { get; }
    }
}