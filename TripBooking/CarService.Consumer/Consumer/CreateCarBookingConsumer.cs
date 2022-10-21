using CarService.Domain.Entities;
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
    public class CreateCarBookingConsumer : IConsumer<ICreateCarBookingEvent>
    {
        private readonly CarDbContext _dbContext;

        public CreateCarBookingConsumer(CarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ICreateCarBookingEvent> context)
        {
            //Call DB - Create Car Booking
            //Check dates - if not available publish Failed
            //Publish Created/Create Car Booking

            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Let's create Car Booking for Booking ID - " + context.Message.BookingId);

            Rent rent = new Rent
            {
                Agency = "Agencija",
                Created = DateTime.Now,
                CreatedBy = "TEST",
                DriverAge = 20,
                DropOffDate = DateTime.Now.AddDays(5),
                PickUpDate = DateTime.Now,
                PickUpLocation = "Belgija"
            };

            _dbContext.Rents.Add(rent);
            _dbContext.SaveChanges();

            await context.Publish<IHotelBookingFailedEvent>(new
            {
                CreatedDate = DateTime.Now,
                context.Message.BookingId
            });

            //await context.Publish<IHotelBookingCompletedEvent>(new
            //{
            //    CreatedDate = DateTime.Now,
            //    context.Message.BookingId
            //});
        }
    }
}
