using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System.Threading.Tasks;

namespace HotelService.Consumer.Consumer
{
    public class BookingCreatedConsumer : IConsumer<IBookingCreatedEventModel>
    {
        public async Task Consume(ConsumeContext<IBookingCreatedEventModel> context)
        {
            //Publish event
        }
    }
}
