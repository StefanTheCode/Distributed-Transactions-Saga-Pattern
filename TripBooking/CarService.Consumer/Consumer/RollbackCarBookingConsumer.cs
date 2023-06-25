using CarService.Infrastructure.Persistence;
using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Threading.Tasks;

namespace CarService.Consumer.Consumer
{
    public class RollbackCarBookingConsumer : IConsumer<IRollbackCarBookingEvent>
    {
        private readonly CarDbContext _dbContext;

        public RollbackCarBookingConsumer(CarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IRollbackCarBookingEvent> context)
        {
            //Call DB - Create Car Booking
            //Check dates - if not available publish Failed
            //Publish Created/Create Car Booking

            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Rollback Car Booking for Booking ID - " + context.Message.BookingId);

            //await context.Publish<IHotelBookingCompletedEventModel>(new
            //{
            //    CreatedDate = DateTime.Now,
            //    BookingId = context.Message.BookingId,
            //    FlightId = context.Message.FlightId
            //});

            await context.Publish<ICreateNotificationEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                CorrelationId = context.Message.CorrelationId
            });
        }
    }
}
