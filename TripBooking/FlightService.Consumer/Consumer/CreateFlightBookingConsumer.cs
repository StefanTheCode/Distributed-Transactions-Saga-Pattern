using FlightService.Infrastructure.Persistence;
using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightService.Consumer.Consumer
{
    public class CreateFlightBookingConsumer : IConsumer<ICreateFlightBookingEventModel>
    {
        private readonly FlightDbContext _dbContext;

        public CreateFlightBookingConsumer(FlightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ICreateFlightBookingEventModel> context)
        {
            //Call DB - Create Flight Booking
            //Check dates - if not available publish HotelBookingFailed
            //Publish Created/Create Car Booking

            Console.WriteLine("Let's create Flight Booking for Booking ID - " + context.Message.BookingId);
        }
    }
}
