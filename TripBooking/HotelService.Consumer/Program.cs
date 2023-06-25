using FlightService.Consumer.Consumer;
using HotelService.Consumer.Consumer;
using HotelService.Infrastructure.Persistence;
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

namespace HotelService.Consumer
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

                    services.AddDbContext<HotelDbContext>(options =>
                            options.UseSqlServer(connectionString));

                    var bus = BusConfiguration.Instance
                            .ConfigureBus(massTransitSettings, (cfg) =>
                            {
                                var context = services.BuildServiceProvider().GetService<HotelDbContext>();

                                cfg.ReceiveEndpoint(nameof(CreatedBookingEvent), e =>
                                {
                                    e.Consumer(() => new HotelBookingCreatedConsumer(context));
                                });

                                cfg.ReceiveEndpoint(nameof(HotelBookingCompletedEvent), e =>
                                {
                                    e.Consumer(() => new HotelBookingCompletedConsumer(context));
                                });

                                cfg.ReceiveEndpoint(nameof(HotelBookingFailedEvent), e =>
                                {
                                    e.Consumer(() => new HotelBookingFailedConsumer(context));
                                });

                                cfg.ReceiveEndpoint(nameof(RollbackHotelBookingEvent), e =>
                                {
                                    e.Consumer(() => new RollbackHotelBookingConsumer(context));
                                });
                            });

                    bus.StartAsync();

                    Console.WriteLine("Hotel Booking Consumer Application started...");
                    Console.ReadLine();
                });
        }
    }
}