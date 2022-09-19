﻿using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Saga.Shared.Consumers.Models.Booking
{
    public class HotelBookingCompletedEvent : IHotelBookingCompletedEvent
    {
        private readonly BookingState _bookingSagaModel;

        public HotelBookingCompletedEvent(BookingState bookingSagaModel)
        {
            _bookingSagaModel = bookingSagaModel;
            CreatedDate = DateTime.Now;
        }

        public Guid CorrelationId => _bookingSagaModel.CorrelationId;
        public DateTime CreatedDate { get; set; }
        public int BookingId => _bookingSagaModel.BookingId;
    }
}