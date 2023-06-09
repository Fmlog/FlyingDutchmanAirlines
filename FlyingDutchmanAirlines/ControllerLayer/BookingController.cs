﻿using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    [Route("{controller}")]
    public class BookingController : Controller
    {
        private readonly BookingService _service;

        public BookingController(BookingService service)
        {
            _service = service;
        }

        [HttpPost("{flightNumber}")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingData body, int flightNumber)
        {
            if (ModelState.IsValid & flightNumber.IsPositive())
            {
                string name = body.FirstName + " " + body.LastName;

                (bool result, Exception? exception) = await _service.CreateBooking(name, flightNumber);

                if (result && exception == null)
                {
                    return StatusCode((int)HttpStatusCode.Created);
                }
                else if (!result && exception != null)
                {
                    return (exception is FlightNotFoundException) ?
                        StatusCode((int)HttpStatusCode.NotFound) : StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
                }
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, ModelState.Root.Errors.First().ErrorMessage);
        }
    }
}
