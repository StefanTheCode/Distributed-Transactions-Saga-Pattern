using HotelService.Infrastructure.Persistence;
using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Threading.Tasks;

namespace FlightService.Consumer.Consumer
{
    public class RollbackHotelBookingConsumer : IConsumer<IRollbackHotelBookingEvent>
    {
        private readonly HotelDbContext _dbContext;

        public RollbackHotelBookingConsumer(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IRollbackHotelBookingEvent> context)
        {
            //Call DB - Create Flight Booking
            //Check dates - if not available publish HotelBookingFailed
            //Publish Created/Create Car Booking

            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Rollback Hotel Booking for Booking ID - " + context.Message.BookingId);

            //await context.Publish<IRollbackCarBookingEvent>(new
            //{
            //    CreatedDate = DateTime.Now,
            //    context.Message.BookingId
            //});

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
