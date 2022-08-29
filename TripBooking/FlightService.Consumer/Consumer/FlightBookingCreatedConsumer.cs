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
    public class FlightBookingCreatedConsumer : IConsumer<IFlightBookingCreatedEventModel>
    {
        private readonly FlightDbContext _dbContext;

        public FlightBookingCreatedConsumer(FlightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IFlightBookingCreatedEventModel> context)
        {
            //Call DB - Create Flight Booking
            //Check dates - if not available publish HotelBookingFailed
            //Publish Created/Create Car Booking

            Console.WriteLine("Flight Booking Consumer - " + context.Message.BookingId);
        }
    }
}
