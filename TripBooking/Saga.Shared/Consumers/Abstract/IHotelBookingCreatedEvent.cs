using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IHotelBookingCreatedEvent
    {
        int BookingId { get; }
        Guid CollerationId { get;}
    }
}