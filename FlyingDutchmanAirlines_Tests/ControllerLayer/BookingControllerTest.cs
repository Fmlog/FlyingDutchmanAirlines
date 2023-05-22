using Azure;
using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer
{
    [TestClass]
    public class BookingControllerTest
    {
        private Mock<BookingService> mockService;

        [TestInitialize]
        public void InitializeTest()
        {
            mockService = new Mock<BookingService>();
            mockService.Setup(s => s.CreateBooking("Femi Alogba", 1))
                .ReturnsAsync((true, null));
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            BookingController controller = new BookingController(mockService.Object);
            BookingData body = new BookingData() { FirstName = "Femi", LastName = "Alogba" };

            ObjectResult? response = await controller.CreateBooking(body, 1) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateBooking_Failure_404()
        {
            mockService.Setup(service => service.CreateBooking(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((false, new FlightNotFoundException()));

            BookingController controller = new BookingController(mockService.Object);

            BookingData bookingData = new BookingData
            {
                FirstName = "John",
                LastName = "Doe"
            };

            ObjectResult? response = await controller.CreateBooking(bookingData, 123) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateBooking_Failure_500_()
        {
            string errorMessage = "Some error occurred";
            mockService.Setup(service => service.CreateBooking(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((false, new Exception(errorMessage)));

            BookingController controller = new BookingController(mockService.Object);

            BookingData bookingData = new BookingData
            {
                FirstName = "John",
                LastName = "Doe"
            };

            ObjectResult? response = await controller.CreateBooking(bookingData, 123) as ObjectResult;

            Assert.IsNotNull(response);

            Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual(errorMessage, response.Value);
        }

        [TestMethod]
        public async Task CreateBooking_Failure_500_InvalidInput()
        {
            var controller = new BookingController(mockService.Object);

            var bookingData = new BookingData
            {
                FirstName = null,
                LastName = "Doe"
            };

            ObjectResult? response = await controller.CreateBooking(bookingData, 123) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
