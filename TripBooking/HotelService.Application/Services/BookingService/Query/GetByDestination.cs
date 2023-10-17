using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotelService.Application.Common;
using HotelService.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Application.Services.BookingService.Query;

public class GetByDestination : IRequest<HotelResponse>
{
    public string Destination { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class GetByDestinationHandler : IRequestHandler<GetByDestination, HotelResponse>
{
    private readonly IHotelDbContext _dbContext;

    public GetByDestinationHandler(IHotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HotelResponse> Handle(GetByDestination request, CancellationToken cancellationToken)
    {
        var hotel = await _dbContext.Hotels
            .Where(x => x.City.ToLower() ==  request.Destination.ToLower() &&
                        x.FromDate.Date == request.FromDate.Date && 
                        x.ToDate.Date == request.ToDate.Date)
            .Select(hotel => new HotelResponse
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address + ", " + hotel.City + ", " + hotel.State,
                City = hotel.City,
                Description = hotel.Description,
                PhotoUrl = hotel.Photo,
                Price = hotel.Price
            })
            .FirstOrDefaultAsync(cancellationToken);

        return hotel;
    }
}