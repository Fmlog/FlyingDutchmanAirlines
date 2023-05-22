using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using System.Runtime.ExceptionServices;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly FlightRepository _flightRepository;

        public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository, FlightRepository flightRepository)
        {
            _bookingRepository = bookingRepository;
            _customerRepository = customerRepository;
            _flightRepository = flightRepository;
        }
        public async Task<(bool, Exception?)> CreateBooking(string customerName, int flightNumber)
        {
            try
            {
                Customer customer;
                try
                {
                    customer = await GetCustomerFromDatabase(customerName) ?? await AddCustomerToDatabase(customerName);
                    if (!await FlightExistsInDatabase(flightNumber))
                    {
                        throw new CouldNotAddBookingToDatabaseException();
                    }
                }
                catch (FlightNotFoundException)
                {
                    await _customerRepository.CreateCustomer(customerName);

                    return await CreateBooking(customerName, flightNumber);
                }
                await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
                return (true, null);
            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        }
        private async Task<Customer> GetCustomerFromDatabase(string name)
        {
            try
            {
                return await _customerRepository.GetCustomerByName(name);
            }
            catch (CustomerNotFoundException)
            {
                return null;
            }
            catch (Exception exception)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException ?? new Exception()).Throw();
                return null;
            }
        }
        private async Task<Customer> AddCustomerToDatabase(string name)
        {
            await _customerRepository.CreateCustomer(name);
            return await _customerRepository.GetCustomerByName(name);
        }

        private async Task<bool> FlightExistsInDatabase(int flightNumber)
        {
            try
            {
                return await _flightRepository.GetFlightByID(flightNumber) != null;
            }
            catch (FlightNotFoundException)
            {
                return false;
            }
        }
    }
}
