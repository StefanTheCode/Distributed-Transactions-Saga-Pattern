using HotelService.Application.Common;
using HotelService.Application.Common.Models;
using HotelService.Domain.Entities;
using HotelService.Domain.Enums;
using MassTransit;
using MediatR;
using Saga.Core.Concrete.Brokers;
using Saga.Core.Constants;
using Saga.Core.MessageBrokers.Concrete;
using Saga.Shared.Consumers.Models.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelService.Application.Services.BookingService.Command
{
    public class Create : IRequest<Result>
    {
        public string Place { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int AdultsNumber { get; set; }
        public int ChildrenNumber { get; set; }
        public int RoomNumber { get; set; }
    }

    public class CreateHandler : IRequestHandler<Create, Result>
    {
        private readonly IHotelDbContext _dbContext;
        private readonly ISendEndpoint _sendEndpoint;
        private readonly MassTransitSettings _massTransitSettings;

        public CreateHandler(IHotelDbContext dbContext, MassTransitSettings massTransitSettings)
        {
            _dbContext = dbContext;
            _massTransitSettings = massTransitSettings;

            var busInstance = BusConfiguration.Instance.ConfigureBus(_massTransitSettings);
            _sendEndpoint = busInstance.GetSendEndpoint(new Uri($"{_massTransitSettings.Uri}/{SagaConstants.SAGAQUEUENAME}")).Result;
        }

        public async Task<Result> Handle(Create request, CancellationToken cancellationToken)
        {
            //Introduce Mapper
            Booking booking = new Booking
            {
                AdultsNumber = request.AdultsNumber,
                RoomNumber = request.RoomNumber,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                ChildrenNumber = request.ChildrenNumber,
                HotelName = request.HotelName,
                Place = request.Place,
                Status = BookingStatus.WaitingForFlight
            };

            await _dbContext.Bookings.AddAsync(booking);

            if (await _dbContext.SaveChangesAsync(cancellationToken) < 1) throw new Exception("Create Booking Failed!"); //publish event?

            await _sendEndpoint.Send(new BookingRequestEvent
            {
                BookingId = booking.Id,
                CreatedDate = DateTime.Now,
                Email = "UsersEmail",
                From = booking.CheckIn,
                To = booking.CheckOut
            });

            return await _dbContext.SaveChangesAsync(cancellationToken) == 1 ?
                new Result(true, Enumerable.Empty<string>()) :
                new Result(false, new List<string> { "There is an error with adding a booking." });
        }
    }
}