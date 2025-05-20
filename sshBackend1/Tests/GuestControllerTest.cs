//using AutoMapper;
//using FakeItEasy;
//using FluentAssertions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using sshBackend1.Controllers;
//using sshBackend1.Models;
//using sshBackend1.Models.DTOs;
//using sshBackend1.Repository.IRepository;
//using sshBackend1.Services.IServices;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;

//namespace UnitTests.ControllerTests
//{
//    public class GuestControllerTest
//    {
//        private readonly IGuestRepository _dbGuest;
//        private readonly IMapper _mapper;
//        private readonly ICacheService _cacheService;
//        protected APIResponse _apiResponse;

//        public GuestControllerTest()
//        {
//            _dbGuest = A.Fake<IGuestRepository>();
//            _mapper = A.Fake<IMapper>();
//            _cacheService = A.Fake<ICacheService>();
//            _apiResponse = new APIResponse();
//        }

//        // GET ALL GUESTS
//        [Fact]
//        public async Task GetGuests_ReturnsOk()
//        {
//            // Arrange
//            var guests = A.Fake<ICollection<GuestDTO>>();
//            var guestList = A.Fake<List<GuestDTO>>();

//            A.CallTo(() => _cacheService.GetOrAddAsync(
//                A<string>.Ignored,
//                A<Func<Task<IEnumerable<Guest>>>>.Ignored,
//                A<TimeSpan>.Ignored
//            )).Returns(Task.FromResult<IEnumerable<Guest>>(new List<Guest>()));

//            A.CallTo(() => _mapper.Map<List<GuestDTO>>(A<IEnumerable<Guest>>.Ignored)).Returns(guestList);

//            var controller = new GuestController(_dbGuest, _mapper, _cacheService);

//            // Act
//            var result = await controller.GetGuests();
//            var okResult = result.Result as OkObjectResult;

//            // Assert
//            okResult.Should().NotBeNull();
//            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

//            var response = okResult.Value as APIResponse;
//            response.Should().NotBeNull();
//            response!.StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        // GET GUEST: Invalid ID
//        [Fact]
//        public async Task GetGuest_InvalidId_ReturnsBadRequest()
//        {
//            // Arrange
//            int invalidId = -1;
//            var controller = new GuestController(_dbGuest, _mapper, _cacheService);

//            // Act
//            var result = await controller.GetGuest(invalidId);
//            var badRequestResult = result.Result as BadRequestObjectResult;

//            // Assert
//            badRequestResult.Should().NotBeNull();
//            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

//            var response = badRequestResult.Value as APIResponse;
//            response.Should().NotBeNull();
//            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        // GET GUEST: Valid ID
//        [Fact]
//        public async Task GetGuest_ValidId_ReturnsOk()
//        {
//            // Arrange
//            int guestId = 1;
//            var guestEntity = new Guest { GuestId = guestId };
//            var guestDto = new GuestDTO { GuestId = guestId };

//            A.CallTo(() => _dbGuest.GetAsync(
//                A<Expression<Func<Guest, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult(guestEntity));

//            A.CallTo(() => _mapper.Map<GuestDTO>(guestEntity)).Returns(guestDto);

//            var controller = new GuestController(_dbGuest, _mapper, _cacheService);

//            // Act
//            var result = await controller.GetGuest(guestId);
//            var okResult = result.Result as OkObjectResult;

//            // Assert
//            okResult.Should().NotBeNull();
//            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

//            var response = okResult.Value as APIResponse;
//            response.Should().NotBeNull();
//            response!.Result.Should().Be(guestDto);
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        // GET GUEST: Not Found
//        [Fact]
//        public async Task GetGuest_NotFound_ReturnsNotFound()
//        {
//            // Arrange
//            int guestId = 100;

//            A.CallTo(() => _dbGuest.GetAsync(
//                A<Expression<Func<Guest, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult<Guest>(null));

//            var controller = new GuestController(_dbGuest, _mapper, _cacheService);

//            // Act
//            var result = await controller.GetGuest(guestId);
//            var notFoundResult = result.Result as NotFoundObjectResult;

//            // Assert
//            notFoundResult.Should().NotBeNull();
//            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

//            var response = notFoundResult.Value as APIResponse;
//            response.Should().NotBeNull();
//            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }

//        // CREATE GUEST: Valid Input
//        [Fact]
//        public async Task CreateGuest_ValidInput_ReturnsCreatedAtRoute()
//        {
//            // Arrange
//            var createDto = new GuestDTO { GuestName = "John Doe" };
//            var guestEntity = new Guest { GuestId = 1, GuestName = "John Doe" };
//            var guestDto = new GuestDTO { GuestId = 1, GuestName = "John Doe" };

//            A.CallTo(() => _dbGuest.GetAsync(
//                A<Expression<Func<Guest, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult<Guest>(null)); // No duplicate

//            A.CallTo(() => _mapper.Map<Guest>(createDto)).Returns(guestEntity);
//            A.CallTo(() => _dbGuest.CreateAsync(guestEntity)).Returns(Task.CompletedTask);
//            A.CallTo(() => _mapper.Map<GuestDTO>(guestEntity)).Returns(guestDto);

//            var controller = new GuestController(_dbGuest, _mapper, _cacheService);

//            // Act
//            var result = await controller.CreateGuest(createDto);
//            var createdResult = result.Result as CreatedAtRouteResult;

//            // Assert
//            createdResult.Should().NotBeNull();
//            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

//            var response = createdResult.Value as APIResponse;
//            response.Should().NotBeNull();
//            response!.StatusCode.Should().Be(HttpStatusCode.Created);
//            response.Result.Should().BeEquivalentTo(guestDto);
//        }

//        // CREATE GUEST: Duplicate
//        [Fact]
//        public async Task CreateGuest_Duplicate_ReturnsBadRequest()
//        {
//            // Arrange
//            var createDto = new GuestDTO { GuestName = "Duplicate Guest" };
//            var existingGuest = new Guest { GuestId = 1, GuestName = "Duplicate Guest" };

//            A.CallTo(() => _dbGuest.GetAsync(
//                A<Expression<Func<Guest, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult(existingGuest)); // Simulate duplicate

//            var controller = new GuestController(_dbGuest, _mapper, _cacheService);

//            // Act
//            var result = await controller.CreateGuest(createDto);
//            var badRequestResult = result.Result as BadRequestObjectResult;

//            // Assert
//            badRequestResult.Should().NotBeNull();
//            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
//        }
//        [Fact]
//        public async Task UpdateGuest_ValidDto_ReturnsOkWithNoContentStatus()
//        {
//            // Arrange
//            int GuestId = 1;
//            var updateDto = new GuestDTO { GuestId = GuestId, GuestName = "Updated Guest" };
//            var mappedGuest = new Guest { GuestId = GuestId, GuestName = "Updated Guest" };

//            A.CallTo(() => _mapper.Map<Guest>(updateDto)).Returns(mappedGuest);
//            A.CallTo(() => _dbGuest.UpdateGuestAsync(A<Guest>.Ignored)).Returns(Task.FromResult(mappedGuest)); // Change here

//            var controller = new GuestController(_dbGuest, _mapper, _cacheService); // Also change here

//            // Act
//            var result = await controller.UpdateGuest(GuestId, updateDto);
//            var okResult = result.Result as OkObjectResult;

//            // Assert
//            okResult.Should().NotBeNull();
//            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

//            var response = okResult.Value as APIResponse;
//            response.Should().NotBeNull();
//            response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
//            response.IsSuccess.Should().BeTrue();
//        }

//        // CREATE GUEST: Null Input
//        [Fact]
//        public async Task CreateGuest_NullInput_ReturnsBadRequest()
//        {
//            // Arrange
//            var controller = new GuestController(_dbGuest, _mapper, _cacheService);

//            // Act
//            var result = await controller.CreateGuest(null);
//            var badRequestResult = result.Result as BadRequestObjectResult;

//            // Assert
//            badRequestResult.Should().NotBeNull();
//            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
//            badRequestResult.Value.Should().Be("Invalid Guest data.");
//        }
//    }
//}
