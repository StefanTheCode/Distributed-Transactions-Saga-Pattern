using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Car
{
    public class CreateCarBookingEventModel : ICreateCarBookingEventModel
    {
        private readonly BookingSagaModel _bookingSagaModel;

        public CreateCarBookingEventModel(BookingSagaModel bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
        public int BookingId => _bookingSagaModel.BookingId;
        public int FlightId => _bookingSagaModel.FlightId;
        public int CarId => _bookingSagaModel.CarId;
        public DateTime CreatedDate { get; set; }
    }
}