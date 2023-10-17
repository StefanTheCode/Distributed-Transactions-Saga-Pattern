using CarService.Application.Common.Model;
using CarService.Application.Services.RentService.Query;
using CarService.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class CarController : BaseController
    {
        [HttpGet]
        public async Task<CarResponse> Get(string city, DateTime departureDate, DateTime returnDate, int passengersCount)
        {
            return await Mediator.Send(
               new Get
               {
                   City = city,
                   DepartureDate = departureDate,
                   ReturnDate = returnDate,
                   NumberOfPassengers = passengersCount
               });
        }
    }
}