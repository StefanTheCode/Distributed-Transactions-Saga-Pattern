using FlightService.Application.Common.Models;
using FlightService.Application.Services.FlightService.Query;
using FlightService.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightService.Controllers
{
    public class FlightController : BaseController
    {
        [HttpGet]
        public async Task<FlightResponse> Get(string from, string destination, DateTime departure, DateTime returnDate)
        {
            return await Mediator.Send(
                new GetAll
                {
                    From = from,
                    Destination = destination,
                    Departure = departure,
                    Return = returnDate
                });
        }
    }
}