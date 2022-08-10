﻿using MassTransit;
using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Booking;
using Saga.Shared.Consumers.Models.Car;
using Saga.Shared.Consumers.Models.Flight;
using Saga.Shared.Consumers.Models.Sagas;
using System;

namespace Booking.Saga.Consumer
{
    public class BookingSagaState : MassTransitStateMachine<BookingSagaModel>
    {
        public State HotelBookingReceived { get; set; }
        public State HotelBookingCreated { get; set; }
        public State HotelBookingFailed { get; set; }   
        public State CreateFlightBooking { get; set; }
        public State FlightBookingFailed { get; set; }
        public State FlightBookingCreated { get; set; }


        public Event<IBookingSagaEventModel> HotelBookingReceivedEvent { get; set; }
        public Event<IBookingCreatedEventModel> HotelBookingCreatedEvent { get; set; }
        public Event<ICreateFlightBookingEventModel> CreateFlightBookingEvent { get; set; }
        public Event<IFlightBookingFailedEventModel> FlightBookingFailedEvent { get; set; }
        public Event<ICreateCarBookingEventModel> CreateCarBookingEvent { get; set; }


        public BookingSagaState()
        {
            InstanceState(state => state.CurrentState);

            Event(() => HotelBookingReceivedEvent,
                cfg => cfg.CorrelateById(x => x.BookingId, ctx => ctx.Message.BookingId).SelectId(s => Guid.NewGuid()));

            Event(() => HotelBookingCreatedEvent,
                cfg => cfg.CorrelateById(x => x.Message.CollerationId));

            Event(() => CreateFlightBookingEvent, 
                cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => FlightBookingFailedEvent,
             cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Initially(
                When(HotelBookingReceivedEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Saga.BookingId} booking received."))
                .Publish(ctx => new BookingCreatedEventModel(ctx.Saga))
                .TransitionTo(HotelBookingReceived)
            );

            During(HotelBookingReceived,
                When(HotelBookingCreatedEvent)
                .Then(ctx =>
                {
                    ctx.Saga.BookingId = ctx.Message.BookingId;
                })
                .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.BookingId} booking created event triggered."))
                .Publish(ctx => new CreateFlightBookingEventModel(ctx.Saga))
                .TransitionTo(HotelBookingCreated));

            During(HotelBookingCreated,
               When(CreateFlightBookingEvent)
               .Then(ctx =>
               {
                   ctx.Saga.BookingId = ctx.Message.BookingId;
                   ctx.Saga.FlightId = ctx.Message.FlightId;
               })
               .ThenAsync(ctx => Console.Out.WriteLineAsync($"{ctx.Message.FlightId} flight create event triggered."))
               .Publish(ctx => new CreateCarBookingEventModel(ctx.Saga))
               .TransitionTo(FlightBookingCreated));


            SetCompletedWhenFinalized();
        }
    }
}