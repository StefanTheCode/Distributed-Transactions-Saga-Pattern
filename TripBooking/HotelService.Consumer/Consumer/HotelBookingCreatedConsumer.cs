using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class HotelBookingCreatedConsumer : IConsumer<ICreatedBookingEvent>
    {
        public async Task Consume(ConsumeContext<ICreatedBookingEvent> context)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Booking Created Consumer: " + context.Message.BookingId);

            //await context.Publish<ICreateFlightBookingEvent>(new
            //{
            //    CreatedDate = DateTime.Now,
            //    BookingId = context.Message.BookingId,
            //    CorrelationId = context.CorrelationId ?? Guid.Empty
            //});
        }
    }
}
