using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Saga.Core.Concrete.Brokers;
using Saga.Core.Constants;
using Saga.Core.MessageBrokers.Concrete;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Booking.Saga.Consumer
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

                    services.AddScoped<BookingSagaDbContext>();

                    var p = hostContext.Configuration.GetSection("MassTransitSettings");

                    var massTransitSettings = hostContext.Configuration.GetSection("MassTransitSettings")
                                            .Get<MassTransitSettings>();

                    services.AddSingleton(massTransitSettings);

                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddSagaStateMachine<BookingStateMachine, BookingState>()
                            .EntityFrameworkRepository(r =>
                            {
                                r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                                r.AddDbContext<DbContext, BookingSagaDbContext>((provider, builder) =>
                                {
                                    builder.UseSqlServer("Server=localhost;Database=Saga;Trusted_Connection=True;Integrated Security=true;MultipleActiveResultSets=true");
                                });
                            });

                        cfg.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(new Uri(massTransitSettings.Uri), h =>
                            {
                                h.Username(massTransitSettings.Username);
                                h.Password(massTransitSettings.Password);
                            });

                            cfg.ReceiveEndpoint(SagaConstants.SAGAQUEUENAME, e =>
                            {
                                e.ConfigureSaga<BookingState>(context);
                            });
                        });
                    });


                    var _busInstance = BusConfiguration.Instance.ConfigureBus(massTransitSettings);

                    SendEndpoint.Endpoint = _busInstance.GetSendEndpoint(new Uri($"{massTransitSettings.Uri}/{SagaConstants.SAGAQUEUENAME}")).Result;

                    Console.WriteLine("Booking Saga State Machine Application started...");
                    //Console.ReadLine();
                });
        }
    }
}
