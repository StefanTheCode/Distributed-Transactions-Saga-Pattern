using HotelService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Threading.Tasks;

namespace FlightService.Consumer.Consumer
{
    public class RollbackHotelBookingConsumer : IConsumer<IRollbackHotelBookingEvent>
    {
        private readonly HotelDbContext _dbContext;

        public RollbackHotelBookingConsumer(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IRollbackHotelBookingEvent> context)
        {
            var existHotelBooking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == context.Message.BookingId);

            if (existHotelBooking is not null)
            {
                _dbContext.Bookings.Remove(existHotelBooking);
                await _dbContext.SaveChangesAsync();
            }

            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Rollback Hotel Booking for Booking ID - " + context.Message.BookingId);

            await context.Publish<ICreateNotificationEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                CorrelationId = context.Message.CorrelationId,
                IsSuccessful = true,
                Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\nBooking Id={context.Message.BookingId}\nHotel booking successfully rollbacked."
            });
        }
    }
}
