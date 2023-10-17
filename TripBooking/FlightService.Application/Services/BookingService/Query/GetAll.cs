using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlightService.Application.Common;
using FlightService.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Application.Services.FlightService.Query;

public class GetAll : IRequest<FlightResponse>
{
    public string From { get; set; }
    public string Destination { get; set; }
    public DateTime Departure { get; set; }
    public DateTime Return { get; set; }
}

public class GetAllHandler : IRequestHandler<GetAll, FlightResponse>
{
    private readonly IFlightDbContext _dbContext;

    public GetAllHandler(IFlightDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FlightResponse> Handle(GetAll request, CancellationToken cancellationToken)
    {
        var flight = await _dbContext.Flights
                                   .FirstOrDefaultAsync(x => x.From.ToLower() == request.From.ToLower() &&
                                               x.To.ToLower() == request.Destination.ToLower() &&
                                               x.Departure.Date == request.Departure.Date &&
                                               x.Return.Date == request.Return.Date);

        if (flight == null) return null;

        var flightToReturn = new FlightResponse
        {
            Id = flight.Id,
            From = flight.From,
            Destination = flight.To,
            AirportName = flight.AeroportName,
            DepartureLandingTime = flight.DepartureLandingTime,
            DepartureTakeoffTime = flight.DepartureTakeoffTime,
            ReturnLandingTime = flight.ReturnLandingTime,
            ReturnTakeoffTime = flight.ReturnTakeoffTime,
            PlaneCompany = flight.Plane,
            Price = flight.Price.ToString()
        };

        return flightToReturn;
    }
}