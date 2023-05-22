using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class FlightRepository_Test
    {
        private FlyingDutchmanAirlinesContext _context;
        private FlightRepository _repository;
        [TestInitialize]
        public void InitializeTest()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> options =
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("Flying Dutchman").Options;

            _context = new FlyingDutchmanAirlinesContext_AirportStub(options);
            _context.Flights.Add(new Flight
            {
                FlightNumber = 1,
            });
            _context.SaveChanges();
            _repository = new FlightRepository(_context);
        }

        [TestMethod]
        public async Task GetFlightByID_Success()
        {
            Flight flight = await _repository.GetFlightByID(1);
            Assert.IsNotNull(flight);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByID_Fail_InvalidFlightNumber()
        {
            await _repository.GetFlightByID(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByID_Fail_DatabaseException()
        {
            await _repository.GetFlightByID(2);
        }
    }
}
