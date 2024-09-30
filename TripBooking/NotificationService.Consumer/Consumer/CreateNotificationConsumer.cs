using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Saga.Shared.Consumers.Abstract;
using System;
using System.Threading.Tasks;

namespace NotificationService.Consumer
{
    public class CreateNotificationConsumer : IConsumer<ICreateNotificationEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public CreateNotificationConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<ICreateNotificationEvent> context)
        {
            string message = @$"____________________________________________________|
                               Time: {DateTime.Now.ToString("HH:mm:ss.ffffff")}|
                               Correlation Id = {context.CorrelationId}|
                               Booking Id = {{context.Message.BookingId|
                               Message I got: \n{context.Message.Message}|";

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

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}