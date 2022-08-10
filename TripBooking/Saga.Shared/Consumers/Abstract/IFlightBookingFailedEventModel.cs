using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IFlightBookingFailedEventModel
    {
        public int BookingId { get; }
        public int FlightId { get; }
        public Guid CorrelationId { get;}
        public DateTime CreatedDate { get; }
    }
}