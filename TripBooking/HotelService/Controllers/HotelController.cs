using System.Threading.Tasks;
using HotelService.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using HotelService.Application.Services.BookingService.Query;
using System.Collections.Generic;
using HotelService.Application.Common.Models;
using System;

namespace HotelService.Controllers;

public class HotelController : BaseController
{
    [HttpGet]
    public async Task<HotelResponse> Get(string destination, DateTime fromDate, DateTime toDate)
    {
        return await Mediator.Send(new GetByDestination { Destination = destination, FromDate = fromDate, ToDate = toDate});
    }
}
