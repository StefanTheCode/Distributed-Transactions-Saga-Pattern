using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateCarBookingEventModel
    {
        public int BookingId { get; }
        public int FlightId { get; }
        public int CarId { get; }
        public Guid CorrelationId { get;}
        public DateTime CreatedDate { get; }
    }
}