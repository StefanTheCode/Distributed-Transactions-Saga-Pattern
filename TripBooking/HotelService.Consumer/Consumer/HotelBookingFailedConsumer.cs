using MassTransit;
using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class HotelBookingFailedConsumer : IConsumer<IHotelBookingFailedEvent>
    {
        public async Task Consume(ConsumeContext<IHotelBookingFailedEvent> context)
        {
            //Publish event
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Booking Failed Consumer: " + context.Message.BookingId);

            await context.Publish<IRollbackFlightBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId
            });

            //await context.Publish<IRollbackCarBookingEventModel>(new
            //{
            //    CreatedDate = DateTime.Now,
            //    BookingId = context.Message.BookingId
            //});
        }
    }
}
