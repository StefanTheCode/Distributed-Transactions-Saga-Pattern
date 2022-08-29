using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class HotelBookingCreatedConsumer : IConsumer<IHotelBookingCreatedEvent>
    {
        public async Task Consume(ConsumeContext<IHotelBookingCreatedEvent> context)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Booking Created Consumer: " + context.Message.BookingId);
        }
    }
}