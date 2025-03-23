using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.Shared.DTOs;

[TestClass]
public class EventMapperTests
{
    private IMapper<Event, EventDTO> _mapper;

    [TestInitialize]
    public void Setup()
    {
    }

    [TestMethod]
    public void ToEvent_ValidEventDTO_MapsStreetNameCorrectly() 
    {
        // Arrange
        EventDTO eventDto = new EventDTO
        {
            EventId = 1,
            StreetName = "Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Description = "Cleanup",
            DateOfEvent = DateOnly.FromDateTime(DateTime.Today),
            StartTime = TimeOnly.FromTimeSpan(TimeSpan.Zero),
            EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)),
            Status = "Planned"
        };

        // Act
        Event eventModel = _mapper.ConvertToModel(eventDto);

        // Assert
        Assert.AreEqual("Main St", eventModel.StreetName);
    }

    [TestMethod]
    public void ToEventDTO_ValidEvent_MapsEventIdCorrectly()
    {
        // Arrange
        Event eventModel = new Event
        {
            EventId = 1,
            StreetName = "Main St",
            City = "Anytown",
            ZipCode = "12345",
            Country = "USA",
            Description = "Cleanup",
            DateOfEvent = DateOnly.FromDateTime(DateTime.Today),
            StartTime = TimeOnly.FromTimeSpan(TimeSpan.Zero),
            EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)),
            Status = "Planned"
        };

        // Act
        EventDTO eventDto = _mapper.ConvertToDTO(eventModel);

        // Assert
        Assert.AreEqual(1, eventDto.EventId);
    }
}