using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IRollbackFlightBookingEvent
    {
        int BookingId { get; }
        int FlightDetailsId { get; }
        DateTime CreatedDate { get; }
        Guid CorrelationId { get; }
    }
}