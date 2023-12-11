using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Flight
{
    public class RollbackFlightBookingEvent : IRollbackFlightBookingEvent
    {
        private readonly BookingState _bookingSagaModel;

        public RollbackFlightBookingEvent(BookingState bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
        public int BookingId => _bookingSagaModel.BookingId;
        public int FlightDetailsId => _bookingSagaModel.FlightDetailsId;
        public DateTime CreatedDate { get; set; }
    }
}