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
    public class RollbackFlightBookingConsumer : IConsumer<IRollbackFlightBookingEvent>
    {
        private readonly FlightDbContext _dbContext;

        public RollbackFlightBookingConsumer(FlightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IRollbackFlightBookingEvent> context)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Rollback Flight Booking for Booking ID - " + context.Message.BookingId);

            var existFlight = await _dbContext.Flights.FirstOrDefaultAsync(x => x.Id == context.Message.FlightDetailsId);

            if (existFlight is not null)
            {
                _dbContext.Flights.Remove(existFlight);
                await _dbContext.SaveChangesAsync();
            }

            await context.Publish<ICreateNotificationEvent>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                FlightDetailsId = context.Message.FlightDetailsId,
                CorrelationId = context.Message.CorrelationId,
                IsSuccessful = true,
                Message = $"Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}\n\nFlight is successfully rollbacked. \nBooking Id={context.Message.BookingId}\nFlight Id: {context.Message.FlightDetailsId}\n"
            });
        }
    }
}
