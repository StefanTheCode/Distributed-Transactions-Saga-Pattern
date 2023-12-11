using MassTransit;
using Saga.Shared.Consumers.Abstract;
using Saga.Shared.Consumers.Models.Booking;
using Saga.Shared.Consumers.Models.Sagas;
using System;
using System.Threading.Tasks;

namespace Booking.Saga.Consumer
{
    public class BookingStateMachine : MassTransitStateMachine<BookingState>
    {
        private readonly BookingSagaDbContext _dbContext;

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

            Event(() => RollbackHotelBookingEvent,
                cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => RollbackFlightBookingEvent,
                cfg => cfg.CorrelateById(x => x.Message.CorrelationId));

            Event(() => RollbackCarBookingEvent,
                cfg => cfg.CorrelateById(x => x.Message.CorrelationId));
        }

        public string Failed { get; set; } = "Failed";
        public string Finished { get; set; } = "Finished";
        public string Processing { get; set; } = "Processing";
        public string BookingReceived { get; set; } = "BookingReceived";
        public string HotelBookingCreated { get; set; } = "HotelBookingCreated";
        public string HotelBookingCompleted { get; set; } = "HotelBookingCompleted";
        public string FlightBookingCreated { get; set; } = "FlightBookingCreated";
        public string CarBookingCreated { get; set; } = "CarBookingCreated";
        public string NotificationCreated { get; set; } = "NotificationCreated";

        public Event<IBookingRequestEvent> BookingRequestEvent { get; set; }
        public Event<IHotelBookingFailedEvent> HotelBookingFailedEvent { get; set; }
        public Event<IHotelBookingCompletedEvent> HotelBookingCompletedEvent { get; set; }
        public Event<ICreatedBookingEvent> HotelBookingCreatedEvent { get; set; }
        public Event<ICreateFlightBookingEvent> CreateFlightBookingEvent { get; set; }
        public Event<ICreateCarBookingEvent> CreateCarBookingEvent { get; set; }
        public Event<ICreateNotificationEvent> CreateNotificationEvent { get; set; }
        public Event<IRollbackHotelBookingEvent> RollbackHotelBookingEvent { get; set; }

        public Event<IRollbackFlightBookingEvent> RollbackFlightBookingEvent { get; set; }
        public Event<IRollbackCarBookingEvent> RollbackCarBookingEvent { get; set; }

        private Task SaveStateChanges(string currentState, int bookingId, int flightId, int carId, Guid colId)
        {
            Console.WriteLine($"I'm saving State: {currentState}");

            _dbContext.BookingStates.Add(new BookingState
            {
                BookingId = bookingId,
                CarDetailsId = carId,
                CorrelationId = colId,
                CurrentState = currentState,
                FlightDetailsId = flightId,
                Created = DateTime.Now
            });

            return _dbContext.SaveChangesAsync();
        }

        public BookingStateMachine(BookingSagaDbContext dbContext)
        {
            _dbContext = dbContext;

            InstanceState(x => x.CurrentState);

            ConfigureCorrelationIds();

            Initially(
                When(BookingRequestEvent)
                    .Then(ctx =>
                    {
                        ctx.Saga.BookingId = ctx.Message.BookingId;
                        ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                        ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;

                        SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId);
                    })
                    .ThenAsync(ctx => Console.Out.WriteLineAsync($"Initialy: When BookingRequest Event. Hotel Booking Received {ctx.Message.BookingId}. I'm publishing CreateBookingEvent.Current State: {ctx.Saga.CurrentState}"))
                    .Publish(ctx => new CreatedBookingEvent(ctx.Saga))
                    .TransitionTo(State(nameof(BookingReceived)))
                    .Then(ctx => 
                    {
                        SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId);
                    })
            );

            DuringAny(
         When(BookingRequestEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
                 ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                 ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"Booking Request Event: {ctx.Message.BookingId}."))
             .TransitionTo(State(nameof(BookingReceived)))
             .ThenAsync(ctx => SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId)),
         When(HotelBookingCreatedEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
                 ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                 ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"Hotel Booking Created Event: {ctx.Message.BookingId}."))
             .TransitionTo(State(nameof(HotelBookingCreated)))
             .ThenAsync(ctx => SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId)),
         When(CreateFlightBookingEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
                 ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                 ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"Create Flight Booking Event: {ctx.Message.BookingId}."))
             .TransitionTo(State(nameof(FlightBookingCreated)))
             .ThenAsync(ctx => SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId)),
         When(CreateCarBookingEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
                 ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                 ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"Create Car Booking Event: {ctx.Message.BookingId}."))
             .TransitionTo(State(nameof(CarBookingCreated)))
             .ThenAsync(ctx => SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId)),
         When(HotelBookingCompletedEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
                 ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                 ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"Hotel Booking Completed Event: {ctx.Message.BookingId}."))
             .TransitionTo(State(nameof(HotelBookingCompleted)))
             .ThenAsync(ctx => SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId)),
         When(CreateNotificationEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
                 ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                 ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"Create Notification Event: {ctx.Message.BookingId}."))
             .TransitionTo(State(nameof(NotificationCreated)))
             .ThenAsync(ctx => SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId)),
         When(HotelBookingFailedEvent)
             .Then(ctx =>
             {
                 ctx.Saga.BookingId = ctx.Message.BookingId;
                 ctx.Saga.FlightDetailsId = ctx.Message.FlightDetailsId;
                 ctx.Saga.CarDetailsId = ctx.Message.CarDetailsId;
             })
             .ThenAsync(ctx => Console.Out.WriteLineAsync($"Hotel Booking Failed Event: {ctx.Message.BookingId}."))
             .TransitionTo(State(nameof(Failed)))
             .ThenAsync(ctx => SaveStateChanges(ctx.Saga.CurrentState, ctx.Message.BookingId, ctx.Message.FlightDetailsId, ctx.Message.CarDetailsId, ctx.Message.CorrelationId))
            
            );

            SetCompletedWhenFinalized();
        }

    }
}