﻿using Saga.Shared.Common;
using System;

namespace Saga.Shared.Consumers.Abstract
{
    public interface IBookingRequestEvent
    {
        int BookingId { get; set; }
        int FlightDetailsId { get; set; }
        int CarDetailsId { get; set; }
        Guid CorrelationId { get; set; }
        string Email { get; set; }
        DateTime From { get; set; }
        DateTime To { get; set; }
    }
}