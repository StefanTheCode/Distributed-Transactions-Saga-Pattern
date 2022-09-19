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
            Event(() => SendBookingRequestEvent,
               cfg => cfg.CorrelateById(x => x.BookingId, ctx => ctx.Message.BookingId));

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

        public Event<ISendBookingRequestEvent> SendBookingRequestEvent { get; set; }
        public Event<IHotelBookingFailedEvent> HotelBookingFailedEvent { get; set; }
        public Event<IHotelBookingCompletedEvent> HotelBookingCompletedEvent { get; set; }
        public Event<ICreateFlightBookingEvent> CreateFlightBookingEvent { get; set; }
        public Event<ICreateCarBookingEvent> CreateCarBookingEvent { get; set; }
        public Event<ICreateNotificationEvent> CreateNotificationEvent { get; set; }
        public Event<IRollbackFlightBookingEvent> RollbackFlightBookingEvent { get; set; }

        public BookingStateMachine()
        {
            InstanceState(x => x.CurrentState);

            ConfigureCorrelationIds();

            Initially(
                When(SendBookingRequestEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                    ctx.Saga.CorrelationId = ctx.Message.CorrelationId;
                })
                .Then(ctx => Console.Out.WriteLineAsync($"Hotel Booking Created: {ctx.Message.BookingId}. I want to create Flight Booking. State: {ctx.Saga.CurrentState.Name}"))
                .Publish(ctx => new CreateFlightBookingEvent(ctx.Saga))
                .TransitionTo(HotelBookingCreated)
            );

            During(HotelBookingCreated,
                 When(CreateFlightBookingEvent)
                .Then(ctx =>
                {
                   ctx.Saga.BookingId = ctx.Message.BookingId;
                   ctx.Saga.CorrelationId = ctx.Message.CorrelationId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"Flight Booking Created: {ctx.Message.BookingId}.  I want to create Car Booking. State: {ctx.Saga.CurrentState.Name}"))
                .Publish(ctx => new CreateCarBookingEvent(ctx.Saga))
                .TransitionTo(FlightBookingCreated),
                When(HotelBookingFailedEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"Flight Booking Failed: {ctx.Message.BookingId}.  State: {ctx.Saga.CurrentState.Name}"))
                .Publish(ctx => new RollbackFlightBookingEvent(ctx.Saga))
                .TransitionTo(Failed));

            //During(FlightBookingCreated,
            //     When(CreateCarBookingEvent)
            //   .Then(ctx =>
            //   {
            //       ctx.Saga.BookingId = ctx.Message.BookingId;
            //   })
            //   .ThenAsync(ctx => Console.Out.WriteLineAsync($"Flight Booking Created: {ctx.Message.BookingId}.  I want to create Car Booking. State: {ctx.Saga.CurrentState.Name}"))
            //    .Publish(ctx => new HotelBookingCompletedEvent(ctx.Saga))
            //   );

            //During(FlightBookingCreated,
            //   When(CreateCarBookingEvent)
            // .Then(ctx =>
            // {
            //     ctx.Saga.BookingId = ctx.Message.BookingId;
            // })
            // .ThenAsync(ctx => Console.Out.WriteLineAsync($"Car Booking Created: {ctx.Message.BookingId}.  State: {ctx.Saga.CurrentState.Name}"))
            // .TransitionTo(CarBookingCreated)
            // );

            // DuringAny(
            //    When(HotelBookingFailedEvent)
            //.Then(ctx =>
            //{
            //    ctx.Saga.BookingId = ctx.Message.BookingId;
            //})
            //.ThenAsync(ctx => Console.Out.WriteLineAsync($"Flight Booking Failed: {ctx.Message.BookingId}.  State: {ctx.Saga.CurrentState.Name}"))
            // //.Publish(ctx => new RollbackFlightBookingEvent(ctx.Saga))
            //.TransitionTo(Finished)
            //  );

            //During(FlightBookingCreated,
            //   When(CreateCarBookingEvent)
            // .Then(ctx =>
            // {
            //     ctx.Saga.BookingId = ctx.Message.BookingId;
            // })
            // .ThenAsync(ctx => Console.Out.WriteLineAsync($"Car Booking Created: {ctx.Message.BookingId}. State: {ctx.Saga.CurrentState.Name}"))
            //  .Publish(ctx => new CreateCarBookingEvent(ctx.Saga))
            // .TransitionTo(FlightBookingCreated)
            // );

            //DuringAny(
            //    When(CreateFlightBookingEvent)
            //    .Then(ctx => Console.Out.WriteLineAsync($"Flight Booking Failed: {ctx.Message.BookingId}. State: {ctx.Saga.CurrentState.Name}"))
            //    .Publish(ctx => new RollbackFlightBookingEvent(ctx.Saga))
            //    .TransitionTo(Failed)
            //    .Finalize());

            //  During(HotelBookingCreated,
            //      When(CreateFlightBookingEvent)
            //     .Then(ctx =>
            //     {
            //         ctx.Saga.BookingId = ctx.Message.BookingId;
            //         ctx.Saga.FlightId = ctx.Message.FlightId;
            //     })
            //     .ThenAsync(ctx => Console.Out.WriteLineAsync($"Flight Booking Created: {ctx.Message.BookingId}. I want to create Car Booking. State: {ctx.Saga.CurrentState.Name}"))
            //     .Publish(ctx => new CreateCarBookingEvent(ctx.Saga))
            //     .TransitionTo(FlightBookingCreated));

            //  During(FlightBookingCreated,
            //       When(HotelBookingFailedEvent)
            //    .Then(ctx =>
            //    {
            //        ctx.Saga.BookingId = ctx.Message.BookingId;
            //    })
            //    .ThenAsync(ctx => Console.Out.WriteLineAsync($"Car Booking Failed: {ctx.Message.BookingId}. State: {ctx.Saga.CurrentState.Name}"))
            //    //.Publish(ctx => new HotelBookingCompletedEventModel(ctx.Saga))
            //    .TransitionTo(Failed),
            //  When(CreateCarBookingEvent)
            //    .Then(ctx =>
            //    {
            //        ctx.Saga.BookingId = ctx.Message.BookingId;
            //        ctx.Saga.FlightId = ctx.Message.FlightId;
            //    })
            //    .ThenAsync(ctx => Console.Out.WriteLineAsync($"Car Booking Created: {ctx.Message.BookingId}. State: {ctx.Saga.CurrentState.Name}"))
            //    //.Publish(ctx => new HotelBookingCompletedEventModel(ctx.Saga))
            //    .TransitionTo(CarBookingCreated));

            //  During(CarBookingCreated,
            //When(HotelBookingCompletedEvent)
            //  //.Then(ctx =>
            //  //{
            //  //    ctx.Saga.BookingId = ctx.Message.BookingId;
            //  //})
            //  .ThenAsync(ctx => Console.Out.WriteLineAsync($"Hotel Completed Booking Created: {ctx.Message.BookingId}. State: {ctx.Saga.CurrentState.Name}"))
            //  .Publish(ctx => new CreateNotificationEvent(ctx.Saga))
            //  .TransitionTo(Finished)
            //  .Finalize());

            //DuringAny(When(HotelBookingCompletedEvent).ThenAsync(ctx => Console.Out.WriteLineAsync("DOBRO JE u COMPLETED - STATE " + ctx.Saga.CurrentState.Name))
            //    .Publish(ctx => new CreateNotificationEvent(ctx.Saga))
            //    .TransitionTo(Finished).Finalize());
            /////
            //During(FlightBookingCreated,
            //   When(HotelBookingCompletedEvent)
            //  .Then(ctx =>
            //  {
            //      ctx.Saga.BookingId = ctx.Message.BookingId;
            //  })
            //  .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Hotel Booking Completed Event triggered. State: {ctx.Saga.CurrentState.Name}"))
            //  //.Publish(ctx => new CreateNotificationEvent(ctx.Saga))
            //  //.Finalize()
            //  //.TransitionTo(Finished)
            //  );


            ////DuringAny(When(HotelBookingCreatedEvent).ThenAsync(ctx => Console.Out.WriteLineAsync("GRESKA u HotelBookingCreatedEvent - STATE " + ctx.Saga.CurrentState.ToString())).Publish(ctx => new HotelBookingFailedEventModel(ctx.Saga))
            ////    .TransitionTo(BookingFailed).Finalize());

            //DuringAny(When(HotelBookingFailedEvent)
            //    .ThenAsync(ctx => Console.Out.WriteLineAsync("GRESKA u HotelBookingFailedEvent - STATE " + ctx.Saga.CurrentState.ToString()))
            //    .Publish(ctx => new HotelBookingFailedEventModel(ctx.Saga))
            //   .TransitionTo(BookingFailed).Finalize());

            ////DuringAny(When(CreateCarBookingEvent).ThenAsync(ctx => Console.Out.WriteLineAsync("GRESKA u CreateCarBookingEvent - STATE " + ctx.Saga.CurrentState.ToString())).Publish(ctx => new HotelBookingFailedEventModel(ctx.Saga))
            ////  .TransitionTo(BookingFailed).Finalize());

            SetCompletedWhenFinalized();
        }
    }
}