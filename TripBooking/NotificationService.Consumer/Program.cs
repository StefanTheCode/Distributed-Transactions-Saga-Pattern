using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Consumer;
using NotificationService.Infrastructure.Persistence;
using Saga.Core.Concrete.Brokers;
using Saga.Core.MessageBrokers.Concrete;
using Saga.Shared.Consumers.Models.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
builder.Services.AddCors(x => x.AddPolicy("AllowAll", p =>
{ p.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials(); }));

builder.Services.AddOptions();

var massTransitSettings = builder.Configuration.GetSection("MassTransitSettings")
    .Get<MassTransitSettings>();
builder.Services.AddSingleton(massTransitSettings);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NotificationDbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddScoped<CreateNotificationConsumer>();

builder.Services.AddSignalR();

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateNotificationConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint(nameof(CreateNotificationEvent), e =>
        {
            e.ConfigureConsumer<CreateNotificationConsumer>(context);
        });
    });
});


//var serviceProvider = builder.Services.BuildServiceProvider();

//var bus = BusConfiguration.Instance
//    .ConfigureBus(massTransitSettings, (cfg) =>
//    {
//        cfg.ReceiveEndpoint(nameof(CreateNotificationEvent), e =>
//        {
//            // Resolve the consumer using the service provider
//            e.Consumer(() => serviceProvider.GetRequiredService<CreateNotificationConsumer>());
//        });
//    });

builder.Services.AddControllers();

//await bus.StartAsync();

Console.WriteLine("Notification Consumer Application started...");

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notifications");
});

app.Run();

