using CarService.Domain.Entities;
using CarService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Consumer.Consumer
{
    public class CreateCarBookingConsumer : IConsumer<ICreateCarBookingEvent>
    {
        private readonly CarDbContext _dbContext;

        public CreateCarBookingConsumer(CarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task BookingFailedPublish(ConsumeContext<ICreateCarBookingEvent> context, string reason)
        {
            Console.WriteLine($"Rent is NOT booked successfully.\n Rent Id={context.Message.CarDetailsId}");
            Console.WriteLine($"Reason: {reason}");
            Console.WriteLine($"Correlation Id = {context.CorrelationId}");
            Console.WriteLine("I'm calling Booking Failed Service...\n\n");

            await context.Publish<IHotelBookingFailedEvent>(new
            {
                CreatedDate = DateTime.Now,
                context.Message.BookingId,
                context.Message.FlightDetailsId,
                context.Message.CarDetailsId,
                CorrelationId = context.Message.CorrelationId
            });

            var message = $"Rent is NOT booked successfully.\n Rent Id={context.Message.CarDetailsId}\nReason: {reason}\nCorrelation Id = {context.CorrelationId}\nI'm calling Booking Failed Service...";

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

        private async Task BookingCompletedPublish(ConsumeContext<ICreateCarBookingEvent> context)
        {
            Console.WriteLine($"Rent is successfully booked.\nRent Id={context.Message.CarDetailsId}");
            Console.WriteLine($"Correlation Id = {context.CorrelationId}");
            Console.WriteLine("I'm calling Booking Completed Service...\n\n");

            await context.Publish<IHotelBookingCompletedEvent>(new
            {
                CreatedDate = DateTime.Now,
                context.Message.BookingId,
                CarDetailsId = context.Message.CarDetailsId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CorrelationId = context.Message.CorrelationId
            });

            await context.Publish<ICreateNotificationEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CarDetailsId = context.Message.CarDetailsId,
                CorrelationId = context.Message.CorrelationId,
                IsSuccessful = true,
                Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\nCar rent is successfully booked. \nBooking Id={context.Message.BookingId}\nFlight Id: {context.Message.FlightDetailsId}\n Rent Id: {context.Message.CarDetailsId}\nI'm calling Booking Finishing Service..."
            });
        }

        public async Task Consume(ConsumeContext<ICreateCarBookingEvent> context)
        {
            Console.WriteLine($"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
            Console.WriteLine();
            Console.WriteLine($"Correlation Id: {context.CorrelationId}\nLet's create Car Booking for Booking ID - " + context.Message.BookingId);

            if (context.Message.BookingId == 0 && context.Message.CarDetailsId == 0)
            {
                await BookingFailedPublish(context, "Passed BookingId or CarDetailsId is empty.");
                return;
            }

            var existCar = await _dbContext.Rents.FirstOrDefaultAsync(x => x.Id == context.Message.CarDetailsId);

            //OVDE
            if (existCar is not null)
            {
                await BookingFailedPublish(context, $"There is no Rent with Id={context.Message.CarDetailsId}");
                return;
            }

            var booking = new Booking
            {
                CarDetailsId = context.Message.CarDetailsId
            };

            await _dbContext.Bookings.AddAsync(booking);

            if (await _dbContext.SaveChangesAsync(context.CancellationToken) < 1)
            {
                await BookingFailedPublish(context, $"Rent Booking is not added to the database.");
            }
            else
            {
                await BookingCompletedPublish(context);
            }
        }
    }
}