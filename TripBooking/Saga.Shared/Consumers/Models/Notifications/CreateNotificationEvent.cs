using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Notifications
{
    public class CreateNotificationEvent : ICreateNotificationEvent
    {
        private readonly BookingSagaModel _bookingSagaModel;

        public CreateNotificationEvent(BookingSagaModel bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public DateTime CreatedDate { get; set; }
        public int BookingId => _bookingSagaModel.BookingId;
        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
    }
}