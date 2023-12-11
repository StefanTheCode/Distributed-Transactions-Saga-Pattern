using FlightService.Application.Common.Models;
using FlightService.Domain.Entities;
using FlightService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Core.Concrete.Brokers;
using Saga.Core.Constants;
using Saga.Core.MessageBrokers.Concrete;
using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightService.Consumer.Consumer
{
    public class CreateFlightBookingConsumer : IConsumer<ICreateFlightBookingEvent>
    {
        private readonly FlightDbContext _dbContext;

        public CreateFlightBookingConsumer(FlightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task BookingFailedPublish(ConsumeContext<ICreateFlightBookingEvent> context, string reason)
        {
            Console.WriteLine($"Flight is NOT booked successfully.\n Flight Id={context.Message.FlightDetailsId}");
            Console.WriteLine($"Reason: {reason}");
            Console.WriteLine($"Correlation Id = {context.CorrelationId}");
            Console.WriteLine("I'm calling Booking Failed Service...\n\n");

            await context.Publish<IHotelBookingFailedEvent>(new
            {
                CreatedDate = DateTime.Now,
                context.Message.BookingId,
                context.Message.FlightDetailsId,
                CorrelationId = context.Message.CorrelationId
            });


            var message = $"Flight is NOT booked successfully.\n Flight Id={context.Message.FlightDetailsId}\nReason: {reason}\nCorrelation Id = {context.CorrelationId}\nI'm calling Booking Failed Service...";

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

        private async Task CreateCarPublish(ConsumeContext<ICreateFlightBookingEvent> context)
        {
            Console.WriteLine($"Flight is successfully booked.\nFlight Id={context.Message.FlightDetailsId}");
            Console.WriteLine($"Correlation Id = {context.CorrelationId}");
            Console.WriteLine("I'm calling Car Service...\n\n");

            await context.Publish<ICreateCarBookingEvent>(new
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
                Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\n\nFlight is successfully booked. \nBooking Id={context.Message.BookingId}\nFlight Id: {context.Message.FlightDetailsId}\nI'm calling Car Service..."
            });
        }

        public async Task Consume(ConsumeContext<ICreateFlightBookingEvent> context)
        {
            Console.WriteLine($"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
            Console.WriteLine();
            Console.WriteLine($"Correlation Id: {context.CorrelationId}\nLet's create Flight Booking for Booking ID - " + context.Message.BookingId);

            if (context.Message.BookingId == 0 || context.Message.FlightDetailsId == 0)
            {
                await BookingFailedPublish(context, "Passed BookingId or FlightDetailsId is empty.");
                return;
            }

            var existFlight = await _dbContext.Flights.FirstOrDefaultAsync(x => x.Id == context.Message.FlightDetailsId);

            if (existFlight is null)
            {
                await BookingFailedPublish(context, $"There is no Flight with Id={context.Message.FlightDetailsId}");
                return;
            }

            Booking booking = new Booking
            {
                FlightDetailsId = context.Message.FlightDetailsId
            };

            await _dbContext.Bookings.AddAsync(booking);

            if (await _dbContext.SaveChangesAsync(context.CancellationToken) < 1)
            {
                await BookingFailedPublish(context, $"Flight Booking is not added to the database.");
            }
            else
            {
                await CreateCarPublish(context);
            }
        }
    }
}
