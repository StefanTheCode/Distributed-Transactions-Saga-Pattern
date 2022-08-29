using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class HotelBookingFailedConsumer : IConsumer<IHotelBookingFailedEventModel>
    {
        public async Task Consume(ConsumeContext<IHotelBookingFailedEventModel> context)
        {
            //Publish event
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Booking Failed Consumer: " + context.Message.BookingId);

            //await context.Publish<IHotelBookingCompletedEventModel>(new
            //{
            //    BookingId = context.Message.BookingId,
            //    CorrelationId = context.CorrelationId ?? Guid.Empty
            //});
        }
    }
}
