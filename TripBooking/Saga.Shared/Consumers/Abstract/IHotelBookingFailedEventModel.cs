using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IHotelBookingFailedEventModel
    {
        int BookingId { get; }
        Guid CollerationId { get; }
    }
}