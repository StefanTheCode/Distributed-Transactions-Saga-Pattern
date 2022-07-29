using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IBookingCreatedEventModel
    {
        int BookingId { get; }
        Guid CollerationId { get;}
    }
}