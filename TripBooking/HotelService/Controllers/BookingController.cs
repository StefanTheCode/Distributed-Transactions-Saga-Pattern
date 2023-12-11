using HotelService.Application.Common.Models;
using HotelService.Application.Services.BookingService.Command;
using HotelService.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelService.Controllers
{
    public class BookingController : BaseController
    {
        [HttpPost]
        public async Task<Result> Post(Create newBooking)
        {
            return await Mediator.Send(newBooking);
        }
    }
}