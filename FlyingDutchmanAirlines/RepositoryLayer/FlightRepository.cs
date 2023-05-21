using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using Microsoft.EntityFrameworkCore;


namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class FlightRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;

        public FlightRepository(FlyingDutchmanAirlinesContext context)
        {
            _context = context;
        }

        public async Task<Flight> GetFlightByID(int flightNumber, int originAirportId, int destinationAirportId)
        {
            if ( !originAirportId.IsPositive() || !destinationAirportId.IsPositive())
            {
                Console.WriteLine($"Argument Exception in GetFlightByID! FlightNumber = {flightNumber}, " +
                    $"OriginAirportId = {originAirportId}, destinationAirportId = {destinationAirportId},");
                throw new ArgumentException();
            }
            if (!flightNumber.IsPositive())
            {
                Console.WriteLine($"Argument Exception in GetFlightByID! FlightNumber = {flightNumber}");
                throw new FlightNotFoundException();
            }
            return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ?? throw new FlightNotFoundException();
        }
    }
}
