using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Controllers;
using sshBackend1.Models;
using sshBackend1.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using sshBackend1.Repository.IRepository;

public class EventControllerTests
{
    private readonly Mock<IEventRepository> _mockRepo;
    private readonly EventController _controller;

    public EventControllerTests()
    {
        _mockRepo = new Mock<IEventRepository>();
        _controller = new EventController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllEvents()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllEventsAsync(null))
                 .ReturnsAsync(new List<Event> { new Event { EventId = 1, EventName = "Test Event" } });

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<APIResponse>(okResult.Value);
        var events = Assert.IsAssignableFrom<IEnumerable<Event>>(response.Result);
        Assert.Single(events);
    }

    [Fact]
    public async Task GetById_ReturnsEvent_WhenFound()
    {
        // Arrange
        var testEvent = new Event { EventId = 1, EventName = "Test Event" };
        _mockRepo.Setup(repo => repo.GetEventAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Event, bool>>>()))
                 .ReturnsAsync(testEvent);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<APIResponse>(okResult.Value);
        var ev = Assert.IsType<Event>(response.Result);
        Assert.Equal(1, ev.EventId);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenDeleted()
    {
        // Arrange
        var eventToDelete = new Event { EventId = 1, EventName = "To Delete" };
        _mockRepo.Setup(repo => repo.GetEventAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Event, bool>>>()))
                 .ReturnsAsync(eventToDelete);
        _mockRepo.Setup(repo => repo.DeleteEventAsync(eventToDelete)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<APIResponse>(okResult.Value);
        Assert.True(response.IsSuccess);
    }
}
