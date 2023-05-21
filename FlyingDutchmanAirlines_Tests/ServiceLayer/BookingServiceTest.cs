using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer
{
    [TestClass]
    public class BookingServiceTest
    {
        private Mock<BookingRepository> _mockBookingRepo;
        private Mock<CustomerRepository> _mockCustomerRepo;


        [TestInitialize]
        public void TestInitialize()
        {
            _mockBookingRepo = new Mock<BookingRepository>();
            _mockCustomerRepo = new Mock<CustomerRepository>();
        }

        [TestMethod]
        public async Task CreateBoooking_Success()
        {
            _mockBookingRepo.Setup(repo => repo.CreateBooking(0, 0)).Returns(Task.CompletedTask);

            _mockCustomerRepo.Setup(repo => repo.GetCustomerByName("Femi"))
                .Returns(Task.FromResult<Customer>(new Customer("Femi") { CustomerId = 0 }));

            BookingService serivce = new BookingService(_mockBookingRepo.Object, _mockCustomerRepo.Object);

            (bool result, Exception? exception) = await serivce.CreateBooking("Femi", 0);
            Console.WriteLine(exception);
            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }

        [TestMethod]
        [DataRow("", 0)]
        [DataRow(null, -1)]
        [DataRow("Galileo Galilei", -1)]
        public async Task CreateBooking_Fail_InvalidInput(string cName, int fNum)
        {
            BookingService service = new BookingService(_mockBookingRepo.Object, _mockCustomerRepo.Object);
            (bool result, Exception? exception) = await service.CreateBooking(cName, fNum);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public async Task CreateBooking_Failure_RepositoryException_ArgumentException()
        {
            _mockBookingRepo.Setup(repository => repository.CreateBooking(0, 1))
                .Throws(new ArgumentException());

            _mockCustomerRepo.Setup(repository => repository.GetCustomerByName("Galileo Galilei"))
                .Returns(Task.FromResult(new Customer("Galileo Galilei") { CustomerId = 0 }));


            BookingService service = new BookingService(_mockBookingRepo.Object, _mockCustomerRepo.Object);

            (bool result, Exception? exception) = await service.CreateBooking("Galileo Galilei", 1);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));


        }

        [TestMethod]
        public async Task CreateBooking_Failure_RepositoryException_DatabaseException()
        {
            _mockBookingRepo.Setup(repository => repository.CreateBooking(1, 2))
                .Throws(new CouldNotAddBookingToDatabaseException());

            _mockCustomerRepo.Setup(repository => repository.GetCustomerByName("Eise Eisinga"))
                .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));

            BookingService service = new BookingService(_mockBookingRepo.Object, _mockCustomerRepo.Object);

            (bool result, Exception exception) = await service.CreateBooking("Eise Eisinga", 2);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }
    }
}
