using HotelService.Infrastructure.Persistence;
using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class HotelBookingCreatedConsumer : IConsumer<ICreatedBookingEvent>
    {
        private readonly HotelDbContext _dbContext;

        public HotelBookingCreatedConsumer(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ICreatedBookingEvent> context)
        {
            Console.WriteLine($"ColID: {context.CorrelationId} - {DateTime.Now.ToString("HH:mm:ss.ffffff")}: Booking Created Consumer: " + context.Message.BookingId);

            if(true)
            {
                await context.Publish<ICreateFlightBookingEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    CorrelationId = context.CorrelationId ?? Guid.Empty
                });
            }
            else
            {
                await context.Publish<IHotelBookingFailedEvent>(new
                {
                    CreatedDate = DateTime.Now,
                    BookingId = context.Message.BookingId,
                    CorrelationId = context.CorrelationId ?? Guid.Empty
                });
            }
        }
    }
}
