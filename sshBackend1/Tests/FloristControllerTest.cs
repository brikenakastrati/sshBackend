using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Controllers;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class FloristControllerTest
    {
        private readonly IFloristRepository _dbFlorist;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public FloristControllerTest()
        {
            _dbFlorist = A.Fake<IFloristRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }

        // GET ALL FLORISTS
        [Fact]
        public async Task GetFlorists_ReturnsOk()
        {
            // Arrange
            var florists = A.Fake<ICollection<FloristDTO>>();
            var floristList = A.Fake<List<FloristDTO>>();

            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored,
                A<Func<Task<IEnumerable<Florist>>>>.Ignored,
                A<TimeSpan>.Ignored
            )).Returns(Task.FromResult<IEnumerable<Florist>>(new List<Florist>()));

            A.CallTo(() => _mapper.Map<List<FloristDTO>>(A<IEnumerable<Florist>>.Ignored)).Returns(floristList);

            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlorists();
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // GET FLORIST: Invalid ID
        [Fact]
        public async Task GetFlorist_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlorist(invalidId);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var response = badRequestResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // GET FLORIST: Valid ID
        [Fact]
        public async Task GetFlorist_ValidId_ReturnsOk()
        {
            // Arrange
            int floristId = 1;
            var floristEntity = new Florist { FloristId = floristId };
            var floristDto = new FloristDTO { FloristId = floristId };

            A.CallTo(() => _dbFlorist.GetAsync(
                A<Expression<Func<Florist, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(floristEntity));

            A.CallTo(() => _mapper.Map<FloristDTO>(floristEntity)).Returns(floristDto);

            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlorist(floristId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(floristDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // GET FLORIST: Not Found
        [Fact]
        public async Task GetFlorist_NotFound_ReturnsNotFound()
        {
            // Arrange
            int floristId = 100;

            A.CallTo(() => _dbFlorist.GetAsync(
                A<Expression<Func<Florist, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Florist>(null));

            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlorist(floristId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // CREATE FLORIST: Valid Input
        [Fact]
        public async Task CreateFlorist_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new FloristDTO { Name = "New Florist" };
            var floristEntity = new Florist { FloristId = 1, Name = "New Florist" };
            var floristDto = new FloristDTO { FloristId = 1, Name = "New Florist" };

            A.CallTo(() => _dbFlorist.GetAsync(
                A<Expression<Func<Florist, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Florist>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<Florist>(createDto)).Returns(floristEntity);
            A.CallTo(() => _dbFlorist.CreateAsync(floristEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<FloristDTO>(floristEntity)).Returns(floristDto);

            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.CreateFlorist(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(floristDto);
        }

        // CREATE FLORIST: Duplicate
        [Fact]
        public async Task CreateFlorist_Duplicate_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new FloristDTO { Name = "Duplicate Florist" };
            var existingFlorist = new Florist { FloristId = 1, Name = "Duplicate Florist" };

            A.CallTo(() => _dbFlorist.GetAsync(
                A<Expression<Func<Florist, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingFlorist)); // Simulate duplicate

            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.CreateFlorist(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        // CREATE FLORIST: Null Input
        [Fact]
        public async Task CreateFlorist_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.CreateFlorist(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid florist data.");
        }

        // DELETE FLORIST: Invalid ID
        [Fact]
        public async Task DeleteFlorist_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteFlorist(invalidId);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var response = badRequestResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        //update: 

        // Add this test to your existing FloristControllerTest class
        [Fact]
        public async Task UpdateFlorist_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int floristId = 1;
            var updateDto = new FloristDTO { FloristId = floristId, Name = "Updated Florist" };
            var mappedFlorist = new Florist { FloristId = floristId, Name = "Updated Florist" };

            A.CallTo(() => _mapper.Map<Florist>(updateDto)).Returns(mappedFlorist);
            A.CallTo(() => _dbFlorist.UpdateFloristAsync(A<Florist>.Ignored)).Returns(Task.FromResult(mappedFlorist));




            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateFlorist(floristId, updateDto);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
        }






        // DELETE FLORIST: Not Found
        [Fact]
        public async Task DeleteFlorist_NotFound_ReturnsNotFound()
        {
            // Arrange
            int floristId = 100;
            A.CallTo(() => _dbFlorist.GetAsync(
                A<Expression<Func<Florist, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Florist>(null));

            var controller = new FloristController(_dbFlorist, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteFlorist(floristId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
