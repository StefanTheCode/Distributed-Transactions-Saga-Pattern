using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateCarBookingEvent : IMessage
    {
        public int BookingId { get; }
        public int FlightId { get; }
        public int CarId { get; }
        public DateTime CreatedDate { get; }
    }
}