using MassTransit;
using Microsoft.Extensions.Configuration;
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
    public class BookingProcessorStateMachine : MassTransitStateMachine<BookingSagaModel>
    {
        private void ConfigureCorrelationIds()
        {
            Event(() => SendBookingRequestEvent,
               cfg => cfg.CorrelateById(x => x.BookingId, ctx => ctx.Message.BookingId).SelectId(s => Guid.NewGuid()));

            Event(() => CreateHotelBookingEvent,
                cfg => cfg.CorrelateById(x => x.Message.CollerationId));

            Event(() => HotelBookingCreatedEvent,
               cfg => cfg.CorrelateById(x => x.Message.CollerationId));

            Event(() => HotelBookingFailedEvent,
              cfg => cfg.CorrelateById(x => x.Message.CollerationId));

            Event(() => HotelBookingCompletedEvent,
              cfg => cfg.CorrelateById(x => x.Message.CollerationId));

            Event(() => CreateFlightBookingEvent,
                cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => CreateCarBookingEvent,
             cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => CreateNotificationEvent,
            cfg => cfg.CorrelateById(x => x.Message.CorrelationId));
        }

        //private void UpdateSagaState(BookingSagaModel state)
        //{
        //    var currentDate = DateTime.Now;
        //    state.Created = currentDate;
        //    state.Updated = currentDate;
        //    state.BookingId = state.BookingId;
        //}

        public State Failed { get; set; }
        public State Finished { get; set; }
        public State Processing { get; set; }
        public State BookingReceived { get; set; }
        public State HotelBookingCreated { get; set; }
        public State FlightBookingCreated { get; set; }
        public State CarBookingCreated { get; set; }


        public Event<ISendBookingRequestEvent> SendBookingRequestEvent { get; set; }
        public Event<ICreateHotelBookingEvent> CreateHotelBookingEvent { get; set; }
        public Event<IHotelBookingCreatedEvent> HotelBookingCreatedEvent { get; set; }
        public Event<IHotelBookingFailedEventModel> HotelBookingFailedEvent { get; set; }
        public Event<IHotelBookingCompletedEventModel> HotelBookingCompletedEvent { get; set; }
        public Event<ICreateFlightBookingEventModel> CreateFlightBookingEvent { get; set; }
        public Event<ICreateCarBookingEventModel> CreateCarBookingEvent { get; set; }
        public Event<ICreateNotificationEvent> CreateNotificationEvent { get; set; }


        public BookingProcessorStateMachine()
        {
            State(() => Initial);
            InstanceState(x => x.CurrentState);
            ConfigureCorrelationIds();

            Initially(
                When(SendBookingRequestEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"Send Booking Request event occur. {ctx.Message.BookingId} booking received."))
                .Publish(ctx => new CreateHotelBookingEvent(ctx.Saga))
                .TransitionTo(BookingReceived)
            );

            During(BookingReceived,
                When(CreateHotelBookingEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Booking Created event triggered."))
                .Publish(ctx => new CreateFlightBookingEventModel(ctx.Saga))
                .TransitionTo(HotelBookingCreated)
                );

            During(HotelBookingCreated,
                When(CreateFlightBookingEvent)
               .Then(ctx =>
               {
                   ctx.Saga.BookingId = ctx.Message.BookingId;
                   ctx.Saga.FlightId = ctx.Message.FlightId;
               })
               .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Flight Create Event triggered."))
               .Publish(ctx => new CreateCarBookingEventModel(ctx.Saga))
               .TransitionTo(FlightBookingCreated));

            During(FlightBookingCreated,
               When(CreateCarBookingEvent)
              .Then(ctx =>
              {
                  ctx.Saga.BookingId = ctx.Message.BookingId;
                  ctx.Saga.FlightId = ctx.Message.FlightId;
              })
              .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Car Create Event triggered."))
              .Publish(ctx => new CreateNotificationEvent(ctx.Saga))
              .TransitionTo(CarBookingCreated));

            During(CarBookingCreated,
           When(CreateNotificationEvent)
          .Then(ctx =>
          {
              ctx.Saga.BookingId = ctx.Message.BookingId;
          })
          .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Car Notification Event triggered."))
          .Publish(ctx => new HotelBookingCompletedEventModel(ctx.Saga))
          .Finalize()
          .TransitionTo(Finished));

            //During(FlightBookingCreated,
            //    When(CreateCarBookingEvent)
            //   .Then(ctx =>
            //   {
            //       ctx.Saga.BookingId = ctx.Message.BookingId;
            //       ctx.Saga.FlightId = ctx.Message.FlightId;
            //       ctx.Saga.CarId = ctx.Message.CarId;
            //   })
            //   .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} - Create Car Booking event triggered. "))
            //   .Publish(ctx => new HotelBookingCompletedEventModel(ctx.Saga))
            //   .TransitionTo(CarBookingCreated));


            //During(CarBookingCreated,
            //    When(HotelBookingFailedEvent)
            //    .Then(ctx =>
            //    {
            //        ctx.Saga.BookingId = ctx.Message.BookingId;
            //    })
            //    .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Booking Failed event triggered. Current state: {ctx.Saga.CurrentState}"))
            //    //.TransitionTo(Failed)
            //    .Finalize()
            //    );

            // During(Finished,
            //When(HotelBookingFailedEvent)
            //.Then(ctx =>
            //{
            //    ctx.Saga.BookingId = ctx.Message.BookingId;
            //})
            //.ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Booking Failed event triggered. Current state: {ctx.Saga.CurrentState}"))
            //.TransitionTo(Failed)
            //.Finalize()
            //);

            //During(CarBookingCreated,
            //    //   When(HotelBookingFailedEvent)
            //    // .Then(ctx =>
            //    // {
            //    //     ctx.Saga.BookingId = ctx.Message.BookingId;
            //    // })
            //    //.ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId}  - Create Booking Failed"))
            //    // .Publish(ctx => new HotelBookingFailedEventModel(ctx.Saga))
            //    // .TransitionTo(BookingFailed)
            //    // .Finalize(),
            //    When(HotelBookingCompletedEvent)
            //      .Then(ctx =>
            //      {
            //          ctx.Saga.BookingId = ctx.Message.BookingId;
            //      })
            //    .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} Booking completed event triggered"))
            //    .TransitionTo(Finished)
            //    .Finalize());


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