using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IHotelBookingFailedEvent
    {
        int BookingId { get; }
        int FlightDetailsId { get; }
        int CarDetailsId { get; }
        Guid CorrelationId { get; }
    }
}