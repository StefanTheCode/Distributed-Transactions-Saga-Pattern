using CarService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Rollback Car Booking for Booking ID - " + context.Message.BookingId);

            var existRent = await _dbContext.Rents.FirstOrDefaultAsync(x => x.Id == context.Message.CarDetailsId);

            if (existRent is not null)
            {
                _dbContext.Rents.Remove(existRent);
                await _dbContext.SaveChangesAsync();
            }

            await context.Publish<ICreateNotificationEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CorrelationId = context.Message.CorrelationId,
                IsSuccessful = true,
                Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\n\nCar Rent is successfully rollbacked. \nBooking Id={context.Message.BookingId}\nRent Id: {context.Message.CarDetailsId}\n"
            });
        }
    }
}
