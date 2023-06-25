using MassTransit;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saga.Shared.Consumers.Models.Sagas
{
    public class BookingState : SagaStateMachineInstance
    {
        [Key]
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int CarId { get; set; }
    }
}