using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class FlightService
    {
        private readonly FlightRepository _flightRepository;
        private readonly AirportRepository _airportRepository;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public FlightService()
        {
            if(Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor is reserved for tesing only!");
            }
        }
        public FlightService(FlightRepository flightRepository, AirportRepository airportRepository)
        {
            _flightRepository = flightRepository;
            _airportRepository = airportRepository;
        }

        public virtual async IAsyncEnumerable<FlightView> GetFlights()
        {
            Queue<Flight> flights = _flightRepository.GetFlights();
            foreach (Flight flight in flights)
            {
                Airport origin;
                Airport destn;
                try
                {
                    origin = await _airportRepository.GetAirportByID(flight.Origin);
                    destn = await _airportRepository.GetAirportByID(flight.Destination);
                }
                catch (FlightNotFoundException)
                {
                    throw new FlightNotFoundException();
                }
                catch (Exception)
                {
                    throw new ArgumentException();
                }

                yield return new FlightView(flight.FlightNumber.ToString(), (origin.City, origin.Iata), (destn.City, destn.Iata));
            }
        }

        public virtual async Task<FlightView> GetFlightByFlightNumber(int flightNumber)
        {
            try
            {
                Flight flight = await _flightRepository.GetFlightByID(flightNumber);
                Airport originAirport = await _airportRepository.GetAirportByID(flight.Origin);
                Airport destinationAirport = await _airportRepository.GetAirportByID(flight.Destination);

                return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata), (destinationAirport.City, destinationAirport.Iata));
            }
            catch (FlightNotFoundException)
            {
                throw new FlightNotFoundException();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }
    }
}
