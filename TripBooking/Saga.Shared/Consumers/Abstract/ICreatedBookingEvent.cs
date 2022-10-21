using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreatedBookingEvent
    {
        int BookingId { get; }
        DateTime CreatedDate { get; }
        Guid CorrelationId { get; }
    }
}