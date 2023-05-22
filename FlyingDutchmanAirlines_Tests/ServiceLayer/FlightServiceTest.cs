using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer
{
    [TestClass]
    public class FlightServiceTest
    {
        private Mock<FlightRepository> _mockFlightRepo;
        private Mock<AirportRepository> _mockAirportRepo;

        [TestInitialize]
        public void InitializeTest()
        {
            _mockFlightRepo = new Mock<FlightRepository>();
            _mockAirportRepo = new Mock<AirportRepository>();
            Flight flightInDatabase = new Flight
            {
                FlightNumber = 148,
                Origin = 31,
                Destination = 92
            };

            Queue<Flight> mockReturn = new Queue<Flight>(1);

            mockReturn.Enqueue(flightInDatabase);
            _mockFlightRepo.Setup(repository => repository.GetFlights()).Returns(mockReturn);

            _mockFlightRepo.Setup(repository => repository.GetFlights()).Returns(mockReturn);
            _mockFlightRepo.Setup(repository => repository.GetFlightByID(148))
                .Returns(Task.FromResult(flightInDatabase));

            _mockAirportRepo.Setup(repository =>
             repository.GetAirportByID(31)).ReturnsAsync(new Airport
             {
                 AirportId = 31,
                 City = "Mexico City",
                 Iata = "MEX"
             });
            _mockAirportRepo.Setup(repository =>
             repository.GetAirportByID(92)).ReturnsAsync(new Airport
             {
                 AirportId = 92,
                 City = "Ulaanbaatar",
                 Iata = "UBN"
             });
        }

        [TestMethod]
        public async Task GetFlights_Success()
        {
            FlightService service = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);

            await foreach (FlightView flightView in service.GetFlights())
            {
                Assert.IsNotNull(flightView);
                Assert.AreEqual(flightView.FlightNumber, "148");
                Assert.AreEqual(flightView.Origin.City, "Mexico City");
                Assert.AreEqual(flightView.Origin.Code, "MEX");
                Assert.AreEqual(flightView.Destination.City, "Ulaanbaatar");
                Assert.AreEqual(flightView.Destination.Code, "UBN");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlights_Failure_RepositoryException()
        {
            _mockAirportRepo.Setup(repository => repository.GetAirportByID(31))
                .ThrowsAsync(new FlightNotFoundException());

            FlightService service = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);

            await foreach (FlightView _ in service.GetFlights())
            {
                ;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFlights_Failure_RegularException()
        {
            _mockAirportRepo.Setup(repository => repository.GetAirportByID(31))
                .ThrowsAsync(new NullReferenceException());

            FlightService service = new FlightService(_mockFlightRepo.Object, _mockAirportRepo.Object);
            await foreach (FlightView _ in service.GetFlights())
            {
                ;
            }
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Success()
        {

            FlightService service = new FlightService(_mockFlightRepo.Object,
             _mockAirportRepo.Object);
            FlightView flightView = await service.GetFlightByFlightNumber(148);
            Assert.IsNotNull(flightView);
            Assert.AreEqual(flightView.FlightNumber, "148");
            Assert.AreEqual(flightView.Origin.City, "Mexico City");
            Assert.AreEqual(flightView.Origin.Code, "MEX");
            Assert.AreEqual(flightView.Destination.City, "Ulaanbaatar");
            Assert.AreEqual(flightView.Destination.Code, "UBN");
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlightByFlightNumber_Failure_RepositoryException_FlightNotFoundException()
        {
            _mockFlightRepo.Setup(repository => repository.GetFlightByID(-1)).Throws(new FlightNotFoundException());
            FlightService service = new FlightService(_mockFlightRepo.Object,
             _mockAirportRepo.Object);
            await service.GetFlightByFlightNumber(-1);
        }
    }
}
