using HotelService.Application.Common;
using HotelService.Application.Common.Models;
using HotelService.Domain.Entities;
using MediatR;
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

        public CreateHandler(IHotelDbContext dbContext)
        {
            _dbContext = dbContext;
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
                Place = request.Place
            };

            await _dbContext.Bookings.AddAsync(booking);

            //Publisher to publish Message

            return await _dbContext.SaveChangesAsync(cancellationToken) == 1 ?
                new Result(true, Enumerable.Empty<string>()) :
                new Result(false, new List<string> { "There is an error with adding a booking." });
        }
    }
}