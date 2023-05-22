using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class AirportRepository
    {
        public readonly FlyingDutchmanAirlinesContext _context;

        public AirportRepository(FlyingDutchmanAirlinesContext context)
        {
            _context = context;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public AirportRepository()
        {
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor is reserved for tests only!");
            }
        }
        public virtual async Task<Airport> GetAirportByID(int airportID)
        {
            if (!airportID.IsPositive())
            {
                Console.WriteLine($"Argument Exception in GetAirportByID! AirportID = {airportID}");
                throw new ArgumentException("Invalid Airport Id");
            }

            return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportID) ?? throw new AirportNotFoundException();

        }
    }
}
