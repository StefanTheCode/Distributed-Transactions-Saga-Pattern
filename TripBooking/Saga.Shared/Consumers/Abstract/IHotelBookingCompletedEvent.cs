using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IHotelBookingCompletedEvent : IMessage
    {
        int BookingId { get; }
    }
}