using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ISendBookingRequestEvent
    {
        int BookingId { get; set; }
        string Email { get; set; }
        DateTime From { get; set; }
        DateTime To { get; set; }
        Guid CorrelationId { get; }

    }
}