using FlightService.Infrastructure.Persistence;
using MassTransit;
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
    public class FlightBookingFailedConsumer : IConsumer<IFlightBookingFailedEventModel>
    {
        private readonly FlightDbContext _dbContext;

        public FlightBookingFailedConsumer(FlightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IFlightBookingFailedEventModel> context)
        {
            //Call DB - Create Flight Booking
            //Check dates - if not available publish HotelBookingFailed
            //Publish Created/Create Car Booking

            Console.WriteLine("Flight Booking FAILED Consumer - " + context.Message.BookingId);

            await context.Publish<IHotelBookingFailedEventModel>(new
            {
                CreatedDate = DateTime.Now,
                BookingId = context.Message.BookingId,
                FlightId = context.Message.FlightId
            });
        }
    }
}
