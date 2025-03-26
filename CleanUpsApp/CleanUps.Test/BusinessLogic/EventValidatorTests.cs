using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Validators;
using CleanUps.Shared.DTOs;

[TestClass()]
public class EventValidatorTests
{
    private IValidator<EventDTO> _validator;

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

        // Assert
        Assert.IsTrue(true); // No exception means the test passes
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void ValidateForCreate_MissingStreetName_ThrowsArgumentException()
    {
        // Arrange
        EventDTO eventDto = new EventDTO
        {
            //Missing StreetName
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
    [ExpectedException(typeof(ArgumentException))]
    public void ValidateForCreate_PastDate_ThrowsArgumentException()
    {
        // Arrange
        EventDTO eventDto = new EventDTO
        {
            StreetName = "Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Description = "Cleanup event",
            DateOfEvent = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            StartTime = TimeOnly.FromDateTime(DateTime.Now),
            EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
            Status = "Planned"
        };

        // Act
        _validator.ValidateForCreate(eventDto);

        // Assert (handled by ExpectedException)
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void ValidateForUpdate_NegativeId_ThrowsArgumentException()
    {
        // Arrange
        EventDTO eventDto = new EventDTO
        {
            EventId = -1,
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
        _validator.ValidateForUpdate(-1, eventDto);

        // Assert (handled by ExpectedException)
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void ValidateId_ZeroId_ThrowsArgumentException()
    {
        // Arrange
        int id = 0;

        // Act
        _validator.ValidateId(id);

        // Assert (handled by ExpectedException)
    }
}