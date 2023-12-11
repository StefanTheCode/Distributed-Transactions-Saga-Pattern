using HotelService.Application.Common;
using HotelService.Application.Common.Models;
using HotelService.Domain.Entities;
using HotelService.Domain.Enums;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        public int HotelDetailsId { get; set; }
        public int FlightDetailsId { get; set; }
        public int CarDetailsId { get; set; }
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
            var existingHotel = await _dbContext.Hotels.FirstOrDefaultAsync(x => x.Id == request.HotelDetailsId);

            if (existingHotel is null) return Result.Failure(new List<string> { "Selected Hotel booking is not available anymore." });

            Booking booking = new Booking
            {
                HotelBokingDetailsId = existingHotel.Id
            };

            await _dbContext.Bookings.AddAsync(booking);

            if (await _dbContext.SaveChangesAsync(cancellationToken) < 1) return Result.Failure(new List<string> { "There is an error with adding a booking." });

            await _sendEndpoint.Send(new BookingRequestEvent
            {
                BookingId = booking.Id,
                CreatedDate = DateTime.Now,
                Email = "UsersEmail",
                FlightDetailsId = request.FlightDetailsId,
                CarDetailsId = request.CarDetailsId
            });

            return await _dbContext.SaveChangesAsync(cancellationToken) == 1 ?
                new Result(true, Enumerable.Empty<string>()) :
                new Result(false, new List<string> { "There is an error with adding a booking." });
        }
    }
}