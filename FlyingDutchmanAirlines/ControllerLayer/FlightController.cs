using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    public class FlightController : Controller
    {
        public IActionResult GetFlights()
        {
            return StatusCode((int)HttpStatusCode.OK, "Hello World");
        }
    }
}
