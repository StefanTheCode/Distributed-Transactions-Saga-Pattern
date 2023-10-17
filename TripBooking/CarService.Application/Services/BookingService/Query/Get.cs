using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarService.Application.Common;
using CarService.Application.Common.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarService.Application.Services.RentService.Query;

public class Get : IRequest<CarResponse>
{
    public string City { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int NumberOfPassengers { get; set; }
}

public class GetHandler : IRequestHandler<Get, CarResponse>
{
    private readonly ICarDbContext _dbContext;

    public GetHandler(ICarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CarResponse> Handle(Get request, CancellationToken cancellationToken)
    {
        var car = await _dbContext.Rents
                                      .Where(x => x.MaximumNumOfPassengers > request.NumberOfPassengers &&
                                                  x.City == request.City &&
                                                  x.DeparturePickUp.Date == request.DepartureDate.Date &&
                                                  x.ReturnPickUp.Date == request.ReturnDate.Date)
                                      .Select(x =>
                                   new CarResponse
                                   {
                                       Id = x.Id,
                                       Agency = x.Agency,
                                       CarDescription = x.CarDescription,
                                       CarName = x.CarName,
                                       CarPhoto = x.CarPhoto,
                                       MaximumNumberOfPassengers = x.MaximumNumOfPassengers,
                                       MinimumDriverAge = x.MinimumDriverAge,
                                       PickUpLocation = x.PickUpLocation,
                                       DeparturePickUpTime = x.DeparturePickUp.ToString("HH:mm"),
                                       ReturnPickUpTime = x.ReturnPickUp.ToString("HH:mm")
                                   })
                                   .FirstOrDefaultAsync();

        return car;
    }
}