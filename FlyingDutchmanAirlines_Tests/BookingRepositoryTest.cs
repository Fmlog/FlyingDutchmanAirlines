

using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests
{
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

            _context = new FlyingDutchmanAirlinesContext(contextOptions);
            _repository = new BookingRepository(_context);
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            bool result = _repository.CreateBooking();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateBooking_Fail_DatabaseError()
        {

        }
        [TestMethod]
        public async Task GetBooking_Success()
        {
    
        }
    }
}
