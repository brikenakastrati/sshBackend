using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Controllers;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class EventControllerTest
    {
        private readonly IEventRepository _dbEvent;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public EventControllerTest()
        {
            _dbEvent = A.Fake<IEventRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]

        //GET ALL EVENTS UNIT TEST
        public async Task EventController_GetEvents_ReturnOk()
        {
            // Arrange
            var events = A.Fake<ICollection<EventDTO>>();
            var eventList = A.Fake<List<EventDTO>>();
            A.CallTo(() => _mapper.Map<List<EventDTO>>(events)).Returns(eventList);
            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetEvents(); // ActionResult<APIResponse>
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull(); // Kontrollon që është kthyer 200 OK
            okResult!.Value.Should().BeOfType<APIResponse>(); // Kontrollon tipin e përmbajtjes
        }
        //GET EVENT : 1. Invalid ID, 2. Valid ID, 3. Not found ID

        //1. Invalid ID unit test
        [Fact]
        public async Task GetEvent_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetEvent(invalidId);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var response = badRequestResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //2. Valid ID unit test
        [Fact]
        public async Task GetEvent_WithValidId_ReturnsOk()
        {
            // Arrange
            int eventId = 1;
            var eventEntity = new Event { EventId = eventId }; // përdor instancë reale, jo Fake
            var eventDto = new EventDTO { EventId = eventId };       // e njëjta për DTO

            A.CallTo(() => _dbEvent.GetAsync(
                A<Expression<Func<Event, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult(eventEntity));

            A.CallTo(() => _mapper.Map<EventDTO>(eventEntity)).Returns(eventDto);

            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetEvent(eventId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(eventDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //3. Not found ID unit test
        [Fact]
        public async Task GetEvent_NotFound_ReturnsNotFound()
        {
            // Arrange
            int eventId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbEvent.GetAsync(A<Expression<Func<Event, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<Event>(null));


            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetEvent(eventId);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }



        //CREATE EVENT: 1. Valid Input , 2. Dublicate Event Name, 3. Null EventDTO

        //1. Valid Input unit test
        [Fact]
        public async Task CreateEvent_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new EventDTO { EventName = "Test Event" };
            var createdEntity = new Event { EventId = 1, EventName = "Test Event" };
            var createdDto = new EventDTO { EventId = 1, EventName = "Test Event" };

            A.CallTo(() => _dbEvent.GetAsync(
                A<Expression<Func<Event, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Event>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<Event>(createDto)).Returns(createdEntity);
            A.CallTo(() => _dbEvent.CreateAsync(createdEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<EventDTO>(createdEntity)).Returns(createdDto);

            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var result = await controller.CreateEvent(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(createdDto);
        }
        //2. Dublicate Event Name unit test
        [Fact]
        public async Task CreateEvent_DuplicateEvent_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new EventDTO { EventName = "Duplicate Event" };
            var existingEvent = new Event { EventId = 1, EventName = "Duplicate Event" };

            A.CallTo(() => _dbEvent.GetAsync(
                A<Expression<Func<Event, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingEvent)); // Simulate duplicate

            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var result = await controller.CreateEvent(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        //3. Null EventDTO unit test
        [Fact]
        public async Task CreateEvent_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var result = await controller.CreateEvent(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid event data.");
        }

        //DELETE Event: 1. Invalid ID, 2. Null Entity, 3. Succesful Delete

        //Invalid ID unit testing
        [Fact]
        public async Task DeleteEvent_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var actionResult = await controller.DeleteEvent(invalidId);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var response = badRequestResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //2. Null Entity unit test
        [Fact]
        public async Task DeleteEvent_NullInput_ReturnsBadRequest()
        {

            // Arrange
            int eventId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbEvent.GetAsync(A<Expression<Func<Event, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<Event>(null));
            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteEvent(eventId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert

            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }

        //3. Succesful delete unit test
        [Fact]
        public async Task DeleteEvent_ValidId_ReturnsOkAndNoContentStatus()
        {
            // Arrange
            int eventId = 1;
            var fakeEvent = new Event
            {
                EventId = eventId,
                EventName = "Sample Event"
            };

            A.CallTo(() => _dbEvent.GetAsync(A<Expression<Func<Event, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(fakeEvent));

            A.CallTo(() => _dbEvent.DeleteEventAsync(fakeEvent))
                .Returns(Task.CompletedTask);

            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteEvent(eventId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Also verify that DeleteEventAsync was called
            A.CallTo(() => _dbEvent.DeleteEventAsync(A<Event>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
       public async Task UpdateEvent_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int eventId = 1;
            var updateDto = new EventDTO { EventId = eventId, EventName = "Updated Event" };
            var mappedEvent = new Event { EventId = eventId, EventName = "Updated Event" };

            A.CallTo(() => _mapper.Map<Event>(updateDto)).Returns(mappedEvent);
            A.CallTo(() => _dbEvent.UpdateEventAsync(A<Event>.Ignored)).Returns(Task.FromResult(mappedEvent));

            var controller = new EventController(_dbEvent, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateEvent(eventId, updateDto);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
        }

    };

}