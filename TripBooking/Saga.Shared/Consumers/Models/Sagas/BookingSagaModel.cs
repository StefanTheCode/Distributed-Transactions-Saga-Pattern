﻿using MassTransit;
using System;

namespace Saga.Shared.Consumers.Models.Sagas
{
    public class BookingSagaModel : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public State CurrentState { get; set; }

        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int CarId { get; set; }
    }
}