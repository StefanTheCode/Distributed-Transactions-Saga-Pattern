using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateFlightBookingEventModel
    {
        public int BookingId { get; }
        public Guid CorrelationId { get;}
        public DateTime CreatedDate { get; }
    }
}