using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class BookingRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;
        public BookingRepository(FlyingDutchmanAirlinesContext context)
        {
            _context = context;
        }
        public async Task CreateBooking(int customerID, int flightID)
        {
            if (customerID < 0 || flightID < 0)
            {
                throw new ArgumentException("Invalid arguments provided");
            }
            Booking booking = new Booking
            {
                CustomerId = customerID,
                FlightNumber = flightID
            };
            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception during database query: {exception.Message}");
                throw new CouldNotAddBookingToDatabaseException();
            }

        }
        public async Task<Booking> GetBooking()
        {
            return new Booking();
        }
    }
}
