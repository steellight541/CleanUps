using Microsoft.VisualStudio.TestTools.UnitTesting;
using CleanUps.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanUps.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace CleanUps.API.Controllers.Tests
{
    [TestClass()]
    public class EventAttendancesControllerTests
    {
        //Mock data? Integration testing using Postman?
        //https://stackoverflow.com/questions/3459287/whats-the-difference-between-a-mock-stub


        private EventAttendancesController eventAttendanceConTest;
        private readonly IEventAttendanceService _eventAttendanceService;
        private readonly ILogger<EventAttendancesController> _logger;

        public EventAttendancesControllerTests()
        {
            eventAttendanceConTest = new EventAttendancesController(_eventAttendanceService, _logger);
        }

        [TestMethod()]
        public void EventAttendancesControllerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetEventsForASingleUserAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUsersForASingleEventAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAsyncTest()
        {
            Assert.Fail();
        }
    }
}