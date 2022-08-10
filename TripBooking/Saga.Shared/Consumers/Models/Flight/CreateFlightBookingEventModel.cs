using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Flight
{
    public class CreateFlightBookingEventModel : ICreateFlightBookingEventModel
    {
        private readonly BookingSagaModel _bookingSagaModel;

        public CreateFlightBookingEventModel(BookingSagaModel bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
        public int BookingId => _bookingSagaModel.BookingId;
        public int FlightId => _bookingSagaModel.FlightId;
        public DateTime CreatedDate { get; set; }
    }
}