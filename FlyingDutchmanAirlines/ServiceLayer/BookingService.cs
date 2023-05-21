using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class BookingService
    {
        FlightRepository _flightRepository;
        CustomerRepository _customerRepository;
        public BookingService(FlightRepository flightRepository,  CustomerRepository customerRepository)
        {
            _flightRepository = flightRepository;
            _customerRepository = customerRepository;
        }
        public async Task<bool> CreateBooking(int flightNumber, string customerName)
        {
            if (await _flightRepository.GetFlightByID(flightNumber) == null)
            {
                throw new Exception();
            }

                if (await _customerRepository.GetCustomerByName(customerName) == null)
            {
                await _customerRepository.CreateCustomer(customerName);
            }
            Customer customer = await _customerRepository.GetCustomerByName(customerName);

            return true;
        }
    }
}
