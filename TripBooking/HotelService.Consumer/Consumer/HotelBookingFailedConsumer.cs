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
            Console.WriteLine($"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
            Console.WriteLine();
            Console.WriteLine($"Booking Failed. Booking Id={context.Message.BookingId}");
            Console.WriteLine($"Correlation Id = {context.CorrelationId}");
            Console.WriteLine("I'm calling Rollback Operations...");

            await context.Publish<ICreateNotificationEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CarDetailsId = context.Message.CarDetailsId,
                CorrelationId = context.Message.CorrelationId,
                IsSuccessful = true,
                Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\n\nBooking Failed. Booking Id={context.Message.BookingId}\nI'm calling Rollback Operations..."
            });

            await context.Publish<IRollbackHotelBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
                context.Message.BookingId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CarDetailsId = context.Message.CarDetailsId,
                CorrelationId = context.Message.CorrelationId
            });

            await context.Publish<IRollbackFlightBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
               context.Message.BookingId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CarDetailsId = context.Message.CarDetailsId,
                CorrelationId = context.Message.CorrelationId
            });

            await context.Publish<IRollbackCarBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CarDetailsId = context.Message.CarDetailsId,
                CorrelationId = context.Message.CorrelationId
            });
        }
    }
}
