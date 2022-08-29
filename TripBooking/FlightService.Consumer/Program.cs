using FlightService.Consumer.Consumer;
using FlightService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saga.Core.Concrete.Brokers;
using Saga.Core.MessageBrokers.Concrete;
using Saga.Shared.Consumers.Models.Booking;
using Saga.Shared.Consumers.Models.Flight;
using System;

namespace FlightService.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", true, true);
                config.AddEnvironmentVariables();

                if (args != null)
                    config.AddCommandLine(args);
            })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();

                    var massTransitSettings = hostContext.Configuration.GetSection("MassTransitSettings")
                        .Get<MassTransitSettings>();
                    services.AddSingleton(massTransitSettings);

                    string connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

                    services.AddDbContext<FlightDbContext>(options =>
                            options.UseSqlServer(connectionString));

                    var bus = BusConfiguration.Instance
                            .ConfigureBus(massTransitSettings, (cfg) =>
                            {
                                var context = services.BuildServiceProvider().GetService<FlightDbContext>();
                                cfg.ReceiveEndpoint(nameof(CreateFlightBookingEventModel), e =>
                                {
                                    e.Consumer(() => new CreateFlightBookingConsumer(context));
                                });

                                cfg.ReceiveEndpoint(nameof(FlightBookingFailedEventModel), e =>
                                {
                                    e.Consumer(() => new FlightBookingFailedConsumer(context));
                                });

                                cfg.ReceiveEndpoint(nameof(FlightBookingCreatedEventModel), e =>
                                {
                                    e.Consumer(() => new FlightBookingCreatedConsumer(context));
                                });
                            });

                    bus.StartAsync();

                    Console.WriteLine("Flight Booking Consumer Application started...");
                    Console.ReadLine();
                });
        }
    }
}
