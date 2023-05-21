using FlyingDutchmanAirlines.Data;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class AirportRepositoryTest
    {
        private FlyingDutchmanAirlinesContext _context;
        private AirportRepository _repository;

        [TestInitialize]
        public void InitializeTest()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> options =
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("Flying Dutchman").Options;

            _context = new FlyingDutchmanAirlinesContext_Stub(options);

            SortedList<string, Airport> airports =
                new SortedList<string, Airport>
                {
                    {
                        "PHX",
                        new Airport
                        {
                            AirportId = 1,
                            City = "Phoenix",
                            Iata = "PHX"
                        }
                    },
                    {
                        "DDH",
                        new Airport
                        {
                            AirportId = 2,
                            City = "Bennington",
                            Iata = "DDH"
                        }
                    },
                    {
                        "RDU",
                        new Airport
                        {
                            AirportId = 3,
                            City = "Raleigh-Durham",
                            Iata = "RDU"
                        }
                    }
                };

            Airport airport = new Airport
            {
                AirportId = 1,
                City = "Femi",
                Iata = "FEM"
            };
            _context.Airports.Add(airport);
            _context.SaveChanges();
            _repository = new AirportRepository(_context);
        }

        [TestMethod]
        public async Task GetAirportByID_Success()
        {
            Airport airport = await _repository.GetAirportByID(1);
            Assert.IsNotNull(airport);
            Assert.AreEqual(airport.City, "Femi");
            Assert.AreEqual(airport.Iata, "FEM");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetAirportByID_Fail_InvalidInput()
        {
            using (StringWriter outputStream = new StringWriter())
            {
                try
                {
                    Console.SetOut(outputStream);
                    await _repository.GetAirportByID(-1);

                }
                catch (ArgumentException)
                {
                    Assert.IsTrue(outputStream.ToString().Contains("Argument Exception in GetAirportByID! AirportID = -1"));
                    throw;
                }
            }
        }
        [TestMethod]
        public async Task GetAirportByID_Fail_DatabaseException()
        {
            await Assert.ThrowsExceptionAsync<AirportNotFoundException>(() => _repository.GetAirportByID(10));
        }
    }
}
