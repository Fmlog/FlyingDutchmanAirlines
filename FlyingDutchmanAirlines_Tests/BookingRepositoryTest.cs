using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests
{
    [TestClass]
    public class BookingRepositoryTest
    {
        private FlyingDutchmanAirlinesContext _context;
        private BookingRepository _repository;
        [TestInitialize]
        public async Task InitializeTest()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> contextOptions =
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("Flying Dutchman").Options;

            _context = new FlyingDutchmanAirlinesContext_Stub(contextOptions);
            _repository = new BookingRepository(_context);
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            await _repository.CreateBooking(1, 1);
            Booking booking = _context.Bookings.First();
            Assert.IsNotNull(booking);
            Assert.AreEqual(1, booking.CustomerId);
            Assert.AreEqual(1, booking.FlightNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(CouldNotAddBookingToDatabaseException))]
        public async Task CreateBooking_Fail_DatabaseError()
        {
            await _repository.CreateBooking(0, 1);
        }

        [DataRow(0, -1)]
        [DataRow(-1, 0)]
        [DataRow(-1, -1)]
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task CreateBooking_Fail_InvalidInput(int invalidCustomerID, int invalidFlightNo)
        {
            await _repository.CreateBooking(invalidCustomerID, invalidFlightNo);
        }

        [TestMethod]
        public async Task GetBooking_Success()
        {

        }
    }
}
