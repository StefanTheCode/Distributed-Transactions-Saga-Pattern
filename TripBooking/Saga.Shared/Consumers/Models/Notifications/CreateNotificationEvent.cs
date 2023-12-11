using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Notifications
{
    public class CreateNotificationEvent : ICreateNotificationEvent
    {
        private readonly BookingState _bookingSagaModel;

        public CreateNotificationEvent(BookingState bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public DateTime CreatedDate { get; set; }
        public int BookingId => _bookingSagaModel.BookingId;
        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
        public int FlightDetailsId => _bookingSagaModel.FlightDetailsId;
        public int CarDetailsId => _bookingSagaModel.CarDetailsId;
        public bool IsSuccessful => _bookingSagaModel.IsSuccessful;
        public string Message => _bookingSagaModel.Message;
    }
}