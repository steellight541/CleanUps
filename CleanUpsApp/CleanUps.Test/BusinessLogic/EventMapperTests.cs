using CleanUps.BusinessDomain.Models;
using CleanUps.Shared.DTOs;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Services.Mappers;

[TestClass()]
public class EventMapperTests
{
    private IMapper<Event, EventDTO> _mapper;

    [TestInitialize()]
    public void Setup()
    {
        _mapper = new EventMapper();
    }

    [TestMethod()]
    public void ConvertToDTO_Maps_EventId()
    {
        // Arrange
        var model = new Event { EventId = 1 };
        // Act
        var dto = _mapper.ConvertToDTO(model);
        // Assert
        Assert.AreEqual(1, dto.EventId);
    }

    [TestMethod()]
    public void ConvertToDTO_Maps_StreetName()
    {
        // Arrange
        var model = new Event { StreetName = "Main St" };
        // Act
        var dto = _mapper.ConvertToDTO(model);
        // Assert
        Assert.AreEqual("Main St", dto.StreetName);
    }

    [TestMethod()]
    public void ConvertToDTO_Maps_DateOfEvent()
    {
        // Arrange
        var model = new Event { DateOfEvent = DateOnly.FromDateTime(DateTime.Today) };
        // Act
        var dto = _mapper.ConvertToDTO(model);
        // Assert
        Assert.AreEqual(model.DateOfEvent, dto.DateOfEvent);
    }

    [TestMethod()]
    public void ConvertToModel_Maps_EventId()
    {
        // Arrange
        var dto = new EventDTO { EventId = 1 };
        // Act
        var model = _mapper.ConvertToModel(dto);
        // Assert
        Assert.AreEqual(1, model.EventId);
    }

    [TestMethod()]
    public void ConvertToDTOList_Returns_CorrectCount()
    {
        // Arrange
        var models = new List<Event> { new Event { EventId = 1 }, new Event { EventId = 2 } };
        // Act
        var dtoList = _mapper.ConvertToDTOList(models);
        // Assert
        Assert.AreEqual(2, dtoList.Count);
    }

    [TestMethod()]
    public void ConvertToModelList_Returns_CorrectCount()
    {
        // Arrange
        var listOfDTOs = new List<EventDTO> { new EventDTO { EventId = 1 }, new EventDTO { EventId = 2 } };
        // Act
        var listOfModels = _mapper.ConvertToModelList(listOfDTOs);
        // Assert
        Assert.AreEqual(2, listOfModels.Count);
    }
}