using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IHotelBookingFailedEvent
    {
        int BookingId { get; }
        Guid CorrelationId { get; }
    }
}