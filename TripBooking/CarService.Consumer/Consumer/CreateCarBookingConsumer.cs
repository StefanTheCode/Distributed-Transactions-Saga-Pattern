using CarService.Infrastructure.Persistence;
using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Consumer.Consumer
{
    public class CreateCarBookingConsumer : IConsumer<ICreateCarBookingEventModel>
    {
        private readonly CarDbContext _dbContext;

        public CreateCarBookingConsumer(CarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ICreateCarBookingEventModel> context)
        {
            //Call DB - Create Car Booking
            //Check dates - if not available publish Failed
            //Publish Created/Create Car Booking

            Console.WriteLine("Let's create Car Booking for Booking ID - " + context.Message.BookingId + " Flight Id - " + context.Message.FlightId);
        }
    }
}
