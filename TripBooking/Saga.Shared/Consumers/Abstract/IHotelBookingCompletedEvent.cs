using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IHotelBookingCompletedEvent
    {
        int BookingId { get; }
        Guid CorrelationId { get; }
    }
}