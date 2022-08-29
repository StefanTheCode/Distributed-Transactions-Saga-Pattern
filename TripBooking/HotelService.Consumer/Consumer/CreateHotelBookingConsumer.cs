using MassTransit;
using Saga.Core.Concrete.Brokers;
using Saga.Core.Constants;
using Saga.Core.MessageBrokers.Concrete;
using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Booking;
using Saga.Shared.Consumers.Models.Flight;
using System;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class CreateHotelBookingConsumer : IConsumer<ICreateHotelBookingEvent>
    {
        public CreateHotelBookingConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ICreateHotelBookingEvent> context)
        {
            //Publish event
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Create Hotel Booking Consumer: " + context.Message.BookingId);

            //await context.Publish<CreateFlightBookingEventModel>(new
            //{
            //    BookingId = context.Message.BookingId,
            //    CorrelationId = context.CorrelationId ?? Guid.Empty
            //});
        }
    }
}
