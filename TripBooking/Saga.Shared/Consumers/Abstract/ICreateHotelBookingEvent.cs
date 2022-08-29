using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateHotelBookingEvent : IMessage
    {
        int BookingId { get; }
    }
}