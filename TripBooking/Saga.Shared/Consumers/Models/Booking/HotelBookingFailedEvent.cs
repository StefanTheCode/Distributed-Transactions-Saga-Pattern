using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Shared.Consumers.Models.Booking
{
    public class HotelBookingFailedEvent : IHotelBookingFailedEvent
    {
        private readonly BookingState _bookingSagaModel;

        public HotelBookingFailedEvent(BookingState bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
        public DateTime CreatedDate { get; set; }
        public int BookingId => _bookingSagaModel.BookingId;
    }
}
