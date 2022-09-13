using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class HotelBookingCompletedConsumer : IConsumer<IHotelBookingCompletedEvent>
    {
        public async Task Consume(ConsumeContext<IHotelBookingCompletedEvent> context)
        {
            //Publish event
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Booking Completed Consumer: " + context.Message.BookingId);

            await context.Publish<ICreateNotificationEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                CorrelationId = context.Message.CorrelationId
            });

        }
    }
}
