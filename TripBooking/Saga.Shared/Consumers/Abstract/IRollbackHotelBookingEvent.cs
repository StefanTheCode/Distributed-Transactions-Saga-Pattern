using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IRollbackHotelBookingEvent
    {
        int BookingId { get; }
        DateTime CreatedDate { get; }
        Guid CorrelationId { get; }
    }
}