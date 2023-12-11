using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Car
{
    public class CreateCarBookingEvent : ICreateCarBookingEvent
    {
        private readonly BookingState _bookingSagaModel;

        public CreateCarBookingEvent(BookingState bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
        public int BookingId => _bookingSagaModel.BookingId;
        public int FlightDetailsId => _bookingSagaModel.FlightDetailsId;
        public int CarDetailsId => _bookingSagaModel.CarDetailsId;
        public DateTime CreatedDate { get; set; }
    }
}