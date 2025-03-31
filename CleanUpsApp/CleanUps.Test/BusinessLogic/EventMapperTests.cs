using CleanUps.Shared.DTOs;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Mappers;
using CleanUps.BusinessLogic.Models;

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
        Event model = new Event { EventId = 1 };
        // Act
        EventDTO dto = _mapper.ConvertToDTO(model);
        // Assert
        Assert.AreEqual(1, dto.EventId);
    }

    [TestMethod()]
    public void ConvertToDTO_Maps_StreetName()
    {
        // Arrange
        Event model = new Event { StreetName = "Main St" };
        // Act
        EventDTO dto = _mapper.ConvertToDTO(model);
        // Assert
        Assert.AreEqual("Main St", dto.StreetName);
    }

    [TestMethod()]
    public void ConvertToDTO_Maps_DateOfEvent()
    {
        // Arrange
        Event model = new Event { DateOfEvent = DateOnly.FromDateTime(DateTime.Today) };
        // Act
        EventDTO dto = _mapper.ConvertToDTO(model);
        // Assert
        Assert.AreEqual(model.DateOfEvent, dto.DateOfEvent);
    }

    [TestMethod()]
    public void ConvertToModel_Maps_EventId()
    {
        // Arrange
        EventDTO dto = new EventDTO { EventId = 1 };
        // Act
        Event model = _mapper.ConvertToModel(dto);
        // Assert
        Assert.AreEqual(1, model.EventId);
    }

    [TestMethod()]
    public void ConvertToDTOList_Returns_CorrectCount()
    {
        // Arrange
        List<Event> models = new List<Event> { new Event { EventId = 1 }, new Event { EventId = 2 } };
        // Act
        List<EventDTO> dtoList = _mapper.ConvertToDTOList(models);
        // Assert
        Assert.AreEqual(2, dtoList.Count);
    }

    [TestMethod()]
    public void ConvertToModelList_Returns_CorrectCount()
    {
        // Arrange
        List<EventDTO> listOfDTOs = new List<EventDTO> { new EventDTO { EventId = 1 }, new EventDTO { EventId = 2 } };
        // Act
        List<Event> listOfModels = _mapper.ConvertToModelList(listOfDTOs);
        // Assert
        Assert.AreEqual(2, listOfModels.Count);
    }
}