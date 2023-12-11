using HotelService.Infrastructure.Persistence;
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
        private readonly HotelDbContext _dbContext;

        public HotelBookingCompletedConsumer(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IHotelBookingCompletedEvent> context)
        {
            if (context.Message is not null && context.Message.BookingId is not 0)
            {
                Console.WriteLine("____________________________________________________");
                Console.WriteLine($"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
                Console.WriteLine();
                Console.WriteLine($"Booking is successfully completed. \nBooking Id={context.Message.BookingId}");
                Console.WriteLine($"Correlation Id = {context.CorrelationId}");
                Console.WriteLine("The entire booking is completed.");

                await context.Publish<ICreateNotificationEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    FlightDetailsId = context.Message.FlightDetailsId,
                    CarDetailsId = context.Message.CarDetailsId,
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccessful = true,
                    Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\nBooking Id={context.Message.BookingId}\nFlight Id: {context.Message.FlightDetailsId}\n Rent Id: {context.Message.CarDetailsId}\nAll bookings finished successfully."
                });
            }
            else
            {
                Console.WriteLine("____________________________________________________");
                Console.WriteLine($"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
                Console.WriteLine();
                Console.WriteLine($"Booking is NOT completed succesffully. Booking Id={context.Message.BookingId}");
                Console.WriteLine($"Correlation Id = {context.CorrelationId}");
                Console.WriteLine("I'm calling Booking Failed Service...");

                await context.Publish<IHotelBookingFailedEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    CorrelationId = context.CorrelationId ?? Guid.Empty,
                    FlightDetailsId = context.Message.FlightDetailsId,
                    CarDetailsId = context.Message.CarDetailsId
                });
            }
        }
    }
}
