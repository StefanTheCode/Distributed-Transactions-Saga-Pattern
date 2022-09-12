using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateFlightBookingEvent
    {
        public int BookingId { get; }
        public int FlightId { get; }
        public DateTime CreatedDate { get; }
        public Guid CorrelationId { get; }
    }
}