using MassTransit;
using Microsoft.Extensions.Configuration;
using Saga.Core.Concrete.Brokers;
using Saga.Core.Constants;
using Saga.Core.MessageBrokers.Concrete;
using Saga.Shared.Common;
using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Booking;
using Saga.Shared.Consumers.Models.Car;
using Saga.Shared.Consumers.Models.Flight;
using Saga.Shared.Consumers.Models.Notifications;
using Saga.Shared.Consumers.Models.Sagas;
using System;
using System.Threading.Tasks;

namespace Booking.Saga.Consumer
{
    public class BookingStateMachine : MassTransitStateMachine<BookingState>
    {
        private void ConfigureCorrelationIds()
        {
            Event(() => BookingRequestEvent,
               cfg => cfg.CorrelateById(x => x.BookingId, ctx => ctx.Message.BookingId).SelectId(x => NewId.NextGuid()));

            Event(() => HotelBookingCreatedEvent,
                cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => CreateFlightBookingEvent,
                cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => CreateCarBookingEvent,
             cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => CreateNotificationEvent,
            cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => HotelBookingFailedEvent,
              cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => HotelBookingCompletedEvent,
              cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => RollbackFlightBookingEvent,
           cfg => cfg.CorrelateById(x => x.Message.CorrelationId));
        }

        public State Failed { get; set; }
        public State Finished { get; set; }
        public State Processing { get; set; }
        public State BookingReceived { get; set; }
        public State HotelBookingCreated { get; set; }
        public State FlightBookingCreated { get; set; }
        public State CarBookingCreated { get; set; }

        public Event<IBookingRequestEvent> BookingRequestEvent { get; set; }
        public Event<IHotelBookingFailedEvent> HotelBookingFailedEvent { get; set; }
        public Event<IHotelBookingCompletedEvent> HotelBookingCompletedEvent { get; set; }
        public Event<ICreatedBookingEvent> HotelBookingCreatedEvent { get; set; }
        public Event<ICreateFlightBookingEvent> CreateFlightBookingEvent { get; set; }
        public Event<ICreateCarBookingEvent> CreateCarBookingEvent { get; set; }
        public Event<ICreateNotificationEvent> CreateNotificationEvent { get; set; }
        public Event<IRollbackFlightBookingEvent> RollbackFlightBookingEvent { get; set; }

        public BookingStateMachine()
        {
            InstanceState(x => x.CurrentState);

            ConfigureCorrelationIds();

            Initially(
                When(BookingRequestEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"Hotel Booking Received & Create Event triggered: {ctx.Message.BookingId}. I want to create Flight Booking. State: {ctx.Saga.CurrentState.Name}"))
                .Publish(ctx => new CreatedBookingEvent(ctx.Saga))
                .TransitionTo(BookingReceived)
            );

            During(BookingReceived,
                 When(HotelBookingCreatedEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"Hotel booking Created: {ctx.Message.BookingId}. I want to create Flight Booking. State: {ctx.Saga.CurrentState.Name}"))
                .Publish(ctx => new CreateFlightBookingEvent(ctx.Saga))
                .TransitionTo(HotelBookingCreated));

            During(HotelBookingCreated,
              When(CreateFlightBookingEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
              .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} create flight booking event triggered."))
              .Publish(ctx => new CreateCarBookingEvent(ctx.Saga))
              .TransitionTo(FlightBookingCreated),
              When(HotelBookingFailedEvent)
              .Then(ctx =>
              {
                  ctx.Saga.BookingId = ctx.Message.BookingId;
              })
              .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Flight booking  failed."))
              .Publish(ctx => new HotelBookingFailedEvent(ctx.Saga))
              .TransitionTo(Failed));

            During(FlightBookingCreated,
              When(CreateCarBookingEvent)
              .Then(ctx =>
              {
                  ctx.Saga.BookingId = ctx.Message.BookingId;
              })
              .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Saga.BookingId} create car event triggered."))
              //.Publish(ctx => new HotelBookingCompletedEvent(ctx.Saga))
              .TransitionTo(CarBookingCreated),
              When(HotelBookingFailedEvent)
              .Then(ctx =>
              {
                  ctx.Saga.BookingId = ctx.Message.BookingId;
              })
               .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Saga.BookingId} create car failed."))
               .Publish(ctx => new HotelBookingFailedEvent(ctx.Saga))
               .TransitionTo(Failed));

            During(CarBookingCreated,
            When(HotelBookingCompletedEvent)
            .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Saga.BookingId} booking completed event triggered."))
            .TransitionTo(Finished)
            .Finalize(),
             When(HotelBookingFailedEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Saga.BookingId} create booking failed."))
             .Publish(ctx => new HotelBookingFailedEvent(ctx.Saga))
             .TransitionTo(Failed)
             .Finalize());

            SetCompletedWhenFinalized();
        }
    }
}