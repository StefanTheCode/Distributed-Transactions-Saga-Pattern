using FlightService.Domain.Entities;
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
    public class RollbackFlightBookingConsumer : IConsumer<IRollbackFlightBookingEvent>
    {
        private readonly FlightDbContext _dbContext;

        public RollbackFlightBookingConsumer(FlightDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IRollbackFlightBookingEvent> context)
        {
            //Call DB - Create Flight Booking
            //Check dates - if not available publish HotelBookingFailed
            //Publish Created/Create Car Booking

            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Rollback Flight Booking for Booking ID - " + context.Message.BookingId);

            await context.Publish<IRollbackCarBookingEvent>(new
            {
                CreatedDate = DateTime.Now,
                context.Message.BookingId
            });


            //try
            //{
            //    throw new Exception("Greska");
            //}
            //catch(Exception e)
            //{
            //    //Console.WriteLine("Create Flight Booking went wrong with ID " + context.Message.BookingId);

            //    await context.Publish<IHotelBookingFailedEventModel>(new
            //    {
            //        CreatedDate = DateTime.Now,
            //        BookingId = context.Message.BookingId+1,
            //        FlightId = context.Message.FlightId
            //    });
            //}

        }
    }
}
