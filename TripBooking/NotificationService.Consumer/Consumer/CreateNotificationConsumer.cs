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
            Console.WriteLine("____________________________________________________");
            Console.WriteLine($"\nTime: {DateTime.Now.ToString("HH:mm:ss.ffffff")}");
            Console.WriteLine();
            Console.WriteLine($"Correlation Id = {context.CorrelationId}");
            Console.WriteLine($"Booking Id = {context.Message.BookingId}");
            Console.WriteLine($"Message I got: \n");

            Console.ForegroundColor = context.Message.IsSuccessful ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"{context.Message.Message}");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();
        }
    }
}