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
    public class FlowerArrangementControllerTest
    {
        private readonly IFlowerArrangementRepository _dbFlowerArrangement;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FlowerArrangementControllerTest()
        {
            _dbFlowerArrangement = A.Fake<IFlowerArrangementRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetFlowerArrangements_ReturnsOk()
        {
            var arrangements = A.Fake<ICollection<FlowerArrangement>>();
            var arrangementList = A.Fake<List<FlowerArrangementDTO>>();
            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>._, A<Func<Task<IEnumerable<FlowerArrangement>>>>._, A<TimeSpan>._))
                .Returns(Task.FromResult<IEnumerable<FlowerArrangement>>(arrangements));
            A.CallTo(() => _mapper.Map<List<FlowerArrangementDTO>>(arrangements)).Returns(arrangementList);

            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);

            var result = await controller.GetFlowerArrangements();
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeOfType<APIResponse>();
        }

        [Fact]
        public async Task GetFlowerArrangement_InvalidId_ReturnsBadRequest()
        {
            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);

            var result = await controller.GetFlowerArrangement(-1);
            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
            ((APIResponse)badRequestResult!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //[Fact]
        //public async Task GetFlowerArrangement_WithValidId_ReturnsOk()
        //{
        //    // Arrange
        //    int flowerArrangementId = 1;

        //    var flowerArrangementEntity = new FlowerArrangement { FlowerArrangementId = flowerArrangementId };
        //    var flowerArrangementDto = new FlowerArrangementDTO { FlowerArrangementId = flowerArrangementId };

        //    A.CallTo(() => _dbFlowerArrangement.GetAsync(
        //        A<Expression<Func<FlowerArrangement, bool>>>._, A<bool>._, A<string>._))
        //        .Returns(Task.FromResult(flowerArrangementEntity));

        //    A.CallTo(() => _mapper.Map<FlowerArrangementDTO>(flowerArrangementEntity))
        //        .Returns(flowerArrangementDto);

        //    var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);

        //    // Act
        //    var actionResult = await controller.GetFlowerArrangement(flowerArrangementId);
        //    var okResult = actionResult.Result as OkObjectResult;

        //    // Assert
        //    okResult.Should().NotBeNull();
        //    okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        //    var response = okResult.Value as APIResponse;
        //    response.Should().NotBeNull();
        //    response!.Result.Should().Be(flowerArrangementDto); // Fix: Compare with the instance, not the type
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}

        //[Fact]
        //public async Task GetFlowerArrangement_NotFound_ReturnsNotFound()
        //{
        //    // Arrange
        //    int flowerArrangementId = 100;

        //    A.CallTo(() => _dbFlowerArrangement.GetAsync(
        //        A<Expression<Func<FlowerArrangement, bool>>>._, A<bool>._, A<string>._))
        //        .Returns(Task.FromResult<FlowerArrangement>(null)); // Simulate not found

        //    var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);

        //    // Act
        //    var result = await controller.GetFlowerArrangement(flowerArrangementId);

        //    // Assert
        //    result.Result.Should().BeOfType<NotFoundObjectResult>();

        //    var notFoundResult = result.Result as NotFoundObjectResult;
        //    notFoundResult.Should().NotBeNull();
        //    notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        //    var response = notFoundResult.Value as APIResponse;
        //    response.Should().NotBeNull();
        //    response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //    response.Result.Should().BeNull();
        //}


        [Fact]
        public async Task CreateFlowerArrangement_Valid_ReturnsCreated()
        {
            var createDto = new FlowerArrangementDTO { Name = "New Flower" };
            var entity = new FlowerArrangement { FlowerArrangementId = 1, Name = "New Flower" };

            A.CallTo(() => _dbFlowerArrangement.GetAsync(A<Expression<Func<FlowerArrangement, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangement>(null));
            A.CallTo(() => _mapper.Map<FlowerArrangement>(createDto)).Returns(entity);
            A.CallTo(() => _dbFlowerArrangement.CreateAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<FlowerArrangementDTO>(entity)).Returns(createDto);

            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);
            var result = await controller.CreateFlowerArrangement(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            createdResult.Should().NotBeNull();
            ((APIResponse)createdResult!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateFlowerArrangement_Duplicate_ReturnsBadRequest()
        {
            var dto = new FlowerArrangementDTO { Name = "Duplicate" };
            var entity = new FlowerArrangement { Name = "Duplicate" };

            A.CallTo(() => _dbFlowerArrangement.GetAsync(A<Expression<Func<FlowerArrangement, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);
            var result = await controller.CreateFlowerArrangement(dto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFlowerArrangement_InvalidId_ReturnsBadRequest()
        {
            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);
            var result = await controller.DeleteFlowerArrangement(-1);
            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFlowerArrangement_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbFlowerArrangement.GetAsync(A<Expression<Func<FlowerArrangement, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangement>(null));

            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);
            var result = await controller.DeleteFlowerArrangement(10);
            var notFoundResult = result.Result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateFlowerArrangement_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int FlowerArrangementId = 1;
            var updateDto = new FlowerArrangementDTO { FlowerArrangementId = FlowerArrangementId, Name = "Updated FlowerArrangement" };
            var mappedFlowerArrangement = new FlowerArrangement { FlowerArrangementId = FlowerArrangementId, Name = "Updated FlowerArrangement" };

            A.CallTo(() => _mapper.Map<FlowerArrangement>(updateDto)).Returns(mappedFlowerArrangement);
            A.CallTo(() => _dbFlowerArrangement.UpdateFlowerArrangementAsync(A<FlowerArrangement>.Ignored)).Returns(Task.FromResult(mappedFlowerArrangement));




            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateFlowerArrangement(FlowerArrangementId, updateDto);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteFlowerArrangement_Successful_ReturnsNoContent()
        {
            var entity = new FlowerArrangement { FlowerArrangementId = 1 };

            A.CallTo(() => _dbFlowerArrangement.GetAsync(A<Expression<Func<FlowerArrangement, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbFlowerArrangement.DeleteFlowerArrangementAsync(entity)).Returns(Task.CompletedTask);

            var controller = new FlowerArrangementController(_dbFlowerArrangement, _mapper, _cacheService);
            var result = await controller.DeleteFlowerArrangement(1);
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}