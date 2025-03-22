using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Services;
using CleanUps.Shared.DTOs;
using System;
namespace CleanUps.Test.BusinessLogic
{
    [TestClass()]
    public class EventValidatorTests
    {
        private IEventValidator _validator;

        [TestInitialize()]
        public void Setup()
        {
            _validator = new EventValidator();
        }

        [TestMethod()]
        public void ValidateForCreate_ValidEventDTO_DoesNotThrow()
        {
            // Arrange
            EventDTO eventDto = new EventDTO
            {
                StreetName = "Main St",
                City = "Anytown",
                ZipCode = "12345",
                Country = "USA",
                Description = "Cleanup event",
                DateOfEvent = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
                Status = "Planned"
            };

            // Act
            _validator.ValidateForCreate(eventDto);

            // Assert (implicitly asserts no exception is thrown)
            Assert.IsTrue(true); // Simple pass if no exception
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateForCreate_MissingStreetName_ThrowsArgumentException()
        {
            // Arrange
            var eventDto = new EventDTO
            {
                //Testing if exception is thrown when StreetName is missing
                City = "Anytown",
                ZipCode = "12345",
                Country = "USA",
                Description = "Cleanup event",
                DateOfEvent = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
                Status = "Planned"
            };

            // Act
            _validator.ValidateForCreate(eventDto);

            // Assert (handled by ExpectedException)
        }

        [TestMethod()]
        public void ValidateId_ValidId_DoesNotThrow()
        {
            // Arrange
            int id = 1;

            // Act
            _validator.ValidateId(id);

            // Assert
            Assert.IsTrue(true); // Simple pass if no exception
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateId_NegativeId_ThrowsArgumentException()
        {
            // Arrange
            int id = -1;

            // Act
            _validator.ValidateId(id);

            // Assert (handled by ExpectedException)
        }

    }
}
