using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Booking
{
    public class BookingCreatedEventModel : IBookingCreatedEventModel
    {
        private readonly BookingSagaModel _bookingSagaModel;

        public BookingCreatedEventModel(BookingSagaModel bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public Guid CollerationId => _bookingSagaModel.CorrelationId;
        public DateTime CreatedDate { get; set; }
        public int BookingId => _bookingSagaModel.BookingId;
    }
}