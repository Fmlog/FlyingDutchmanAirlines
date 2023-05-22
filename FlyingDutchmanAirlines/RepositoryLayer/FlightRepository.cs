using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class FlightRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public FlightRepository()
        {
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor reserved for testing only!");
            }
        }
        public FlightRepository(FlyingDutchmanAirlinesContext context)
        {
            _context = context;
        }
        public virtual Queue<Flight> GetFlights()
        {
            Queue<Flight> flights = new Queue<Flight>();
            foreach (Flight flight in _context.Flights)
            {
                flights.Enqueue(flight);
            }
            return flights;
        }
        public virtual async Task<Flight> GetFlightByID(int flightNumber)
        {
            if (!flightNumber.IsPositive())
            {
                Console.WriteLine($"Argument Exception in GetFlightByID! FlightNumber = {flightNumber}");
                throw new FlightNotFoundException();
            }
            return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ?? throw new FlightNotFoundException();
        }
    }
}
