using HotelService.Infrastructure.Persistence;
using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class HotelBookingFailedConsumer : IConsumer<IHotelBookingFailedEvent>
    {
        private readonly HotelDbContext _dbContext;

        public HotelBookingFailedConsumer(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IHotelBookingFailedEvent> context)
        {
            //Publish event
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Booking Failed Consumer: " + context.Message.BookingId);

            await context.Publish<IRollbackHotelBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
                context.Message.BookingId,
                CorrelationId = context.Message.CorrelationId
            });

            await context.Publish<IRollbackFlightBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
               context.Message.BookingId,
                CorrelationId = context.Message.CorrelationId
            });

            await context.Publish<IRollbackCarBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                CorrelationId = context.Message.CorrelationId
            });
        }
    }
}
