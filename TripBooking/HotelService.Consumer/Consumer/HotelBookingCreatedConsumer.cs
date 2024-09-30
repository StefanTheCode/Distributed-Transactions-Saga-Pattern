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
    public class HotelBookingCreatedConsumer : IConsumer<ICreatedBookingEvent>
    {
        private readonly HotelDbContext _dbContext;

        public HotelBookingCreatedConsumer(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ICreatedBookingEvent> context)
        {
            if (context.Message is not null && context.Message.BookingId is not 0)
            {
                Console.WriteLine("____________________________________________________");
                Console.WriteLine($"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
                Console.WriteLine();
                Console.WriteLine($"Hotel is successfully booked. \nBooking Id={context.Message.BookingId}");
                Console.WriteLine($"Correlation Id = {context.CorrelationId}");
                Console.WriteLine("I'm calling Flight Service...\n\n");

                await context.Publish<ICreateFlightBookingEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    FlightDetailsId = context.Message.FlightDetailsId,
                    CarDetailsId = context.Message.CarDetailsId,
                    CorrelationId = context.CorrelationId ?? Guid.Empty
                });

                await context.Publish<ICreateNotificationEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    FlightDetailsId = context.Message.FlightDetailsId,
                    CarDetailsId = context.Message.CarDetailsId,
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccessful = true,
                    Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\n\nHotel is successfully booked. \nBooking Id={context.Message.BookingId}\nI'm calling Flight Service..."
                });
            }
            else
            {
                Console.WriteLine($"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
                Console.WriteLine();
                Console.WriteLine($"Hotel is NOT booked succesffully. \nBooking Id={context.Message.BookingId}");
                Console.WriteLine($"Correlation Id = {context.CorrelationId}");
                Console.WriteLine("I'm calling Booking Failed Service...\n\n");

                await context.Publish<IHotelBookingFailedEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    CorrelationId = context.CorrelationId ?? Guid.Empty,
                    FlightDetailsId = context.Message.FlightDetailsId,
                    CarDetailsId = context.Message.CarDetailsId
                });

                var message = $"Hotel is NOT booked succesffully. \nBooking Id={context.Message.BookingId}\nCorrelation Id = {context.CorrelationId}\nI'm calling Booking Failed Service\n";

                await context.Publish<ICreateNotificationEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    FlightDetailsId = context.Message.FlightDetailsId,
                    CarDetailsId = context.Message.CarDetailsId,
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccessful = false,
                    Message = message
                });
            }
        }
    }
}
