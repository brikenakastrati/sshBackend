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
    public class FlowerArrangementTypeControllerTest
    {
        private readonly IFlowerArrangementTypeRepository _dbFlowerArrangementType;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FlowerArrangementTypeControllerTest()
        {
            _dbFlowerArrangementType = A.Fake<IFlowerArrangementTypeRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetFlowerArrangementTypes_ReturnsOk()
        {
            // Arrange
            var arrangements = A.Fake<ICollection<FlowerArrangementType>>();
            var arrangementList = A.Fake<List<FlowerArrangementTypeDTO>>();
            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>._, A<Func<Task<IEnumerable<FlowerArrangementType>>>>._, A<TimeSpan>._))
                .Returns(Task.FromResult<IEnumerable<FlowerArrangementType>>(arrangements));
            A.CallTo(() => _mapper.Map<List<FlowerArrangementTypeDTO>>(arrangements)).Returns(arrangementList);

            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlowerArrangementTypes();
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeOfType<APIResponse>();
        }

        [Fact]
        public async Task GetFlowerArrangementType_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlowerArrangementType(-1);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            ((APIResponse)badRequestResult!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetFlowerArrangementType_ValidId_ReturnsOk()
        {
            // Arrange
            var entity = new FlowerArrangementType { FlowerArrangementTypeId = 1, Name = "Test" };
            var dto = new FlowerArrangementTypeDTO { FlowerArrangementTypeId = 1, Name = "Test" };

            A.CallTo(() => _dbFlowerArrangementType.GetAsync(A<Expression<Func<FlowerArrangementType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<FlowerArrangementTypeDTO>(entity)).Returns(dto);

            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlowerArrangementType(1);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).Result.Should().Be(dto);
        }

        [Fact]
        public async Task GetFlowerArrangementType_NotFound_ReturnsNotFound()
        {
            // Arrange
            A.CallTo(() => _dbFlowerArrangementType.GetAsync(A<Expression<Func<FlowerArrangementType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangementType>(null));

            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.GetFlowerArrangementType(100);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            ((APIResponse)notFoundResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateFlowerArrangementType_Valid_ReturnsCreated()
        {
            // Arrange
            var createDto = new FlowerArrangementTypeDTO { Name = "New Flower" };
            var entity = new FlowerArrangementType { FlowerArrangementTypeId = 1, Name = "New Flower" };

            A.CallTo(() => _dbFlowerArrangementType.GetAsync(A<Expression<Func<FlowerArrangementType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangementType>(null));
            A.CallTo(() => _mapper.Map<FlowerArrangementType>(createDto)).Returns(entity);
            A.CallTo(() => _dbFlowerArrangementType.CreateAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<FlowerArrangementTypeDTO>(entity)).Returns(createDto);

            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.CreateFlowerArrangementType(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            ((APIResponse)createdResult!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateFlowerArrangementType_Duplicate_ReturnsBadRequest()
        {
            // Arrange
            var dto = new FlowerArrangementTypeDTO { Name = "Duplicate" };
            var entity = new FlowerArrangementType { Name = "Duplicate" };

            A.CallTo(() => _dbFlowerArrangementType.GetAsync(A<Expression<Func<FlowerArrangementType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.CreateFlowerArrangementType(dto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFlowerArrangementType_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteFlowerArrangementType(-1);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFlowerArrangementType_NotFound_ReturnsNotFound()
        {
            // Arrange
            A.CallTo(() => _dbFlowerArrangementType.GetAsync(A<Expression<Func<FlowerArrangementType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangementType>(null));

            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteFlowerArrangementType(10);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
        }

        //controlleri nuk ka metode te UpdateFlowerArrangementType

        //[Fact]
        //public async Task UpdateFlowerArrangementType_ValidDto_ReturnsOkWithNoContentStatus()
        //{
        //    // Arrange
        //    int FlowerArrangementTypeId = 1;
        //    var updateDto = new FlowerArrangementTypeDTO { FlowerArrangementTypeId = FlowerArrangementTypeId, Name = "Updated FlowerArrangementType" };
        //    var mappedFlowerArrangementType = new FlowerArrangementType { FlowerArrangementTypeId = FlowerArrangementTypeId, Name = "Updated FlowerArrangementType" };

        //    A.CallTo(() => _mapper.Map<FlowerArrangementType>(updateDto)).Returns(mappedFlowerArrangementType);
        //    A.CallTo(() => _dbFlowerArrangementType.UpdateFlowerArrangementTypeAsync(A<FlowerArrangementType>.Ignored)).Returns(Task.FromResult(mappedFlowerArrangementType));

        //    var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

        //    // Act
        //    var result = await controller.UpdateFlowerArrangementType(FlowerArrangementTypeId, updateDto);
        //    var okResult = result.Result as OkObjectResult;

        //    // Assert
        //    okResult.Should().NotBeNull();
        //    okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        //    var response = okResult.Value as APIResponse;
        //    response.Should().NotBeNull();
        //    response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        //    response.IsSuccess.Should().BeTrue();
        //}

        [Fact]
        public async Task DeleteFlowerArrangementType_Successful_ReturnsNoContent()
        {
            // Arrange
            var entity = new FlowerArrangementType { FlowerArrangementTypeId = 1 };

            A.CallTo(() => _dbFlowerArrangementType.GetAsync(A<Expression<Func<FlowerArrangementType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbFlowerArrangementType.DeleteFlowerArrangementTypesAsync(entity)).Returns(Task.CompletedTask);

            var controller = new FlowerArrangementTypeController(_dbFlowerArrangementType, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteFlowerArrangementType(1);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
