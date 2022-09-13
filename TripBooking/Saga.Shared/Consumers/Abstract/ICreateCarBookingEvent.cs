using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateCarBookingEvent
    {
        int BookingId { get; }
        int FlightId { get; }
        Guid CorrelationId { get; }
        int CarId { get; }
        DateTime CreatedDate { get; }
    }
}