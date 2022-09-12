using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface ICreateNotificationEvent : IMessage
    {
        int BookingId { get; }
    }
}