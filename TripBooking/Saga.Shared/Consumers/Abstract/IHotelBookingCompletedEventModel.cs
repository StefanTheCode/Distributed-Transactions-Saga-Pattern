using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IHotelBookingCompletedEventModel
    {
        int BookingId { get; }
        Guid CollerationId { get;}
    }
}