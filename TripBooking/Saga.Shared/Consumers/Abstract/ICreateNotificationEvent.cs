using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateNotificationEvent
    {
        Guid CorrelationId { get; }
        int BookingId { get; }
    }
}