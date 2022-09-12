using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IRollbackFlightBookingEvent : IMessage
    {
        public int BookingId { get; }
        public int FlightId { get; }
        public DateTime CreatedDate { get; }
    }
}