using MassTransit;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Threading.Tasks;

namespace NotificationService.Consumer
{
    public class CreateNotificationConsumer : IConsumer<ICreateNotificationEvent>
    {
        public CreateNotificationConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ICreateNotificationEvent> context)
        {
            //Publish event
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffffff")}: Create Notification Consumer: " + context.Message.BookingId);
        }
    }
}