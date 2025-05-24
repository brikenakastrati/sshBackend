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
    public class MusicProviderOrderControllerTest
    {
        private readonly IMusicProviderOrderRepository _dbMusicProviderOrder;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public MusicProviderOrderControllerTest()
        {
            _dbMusicProviderOrder = A.Fake<IMusicProviderOrderRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]

        //GET ALL MusicProviderOrderS UNIT TEST
        public async Task MusicProviderOrderController_GetMusicProviderOrders_ReturnOk()
        {
            // Arrange
            var MusicProviderOrders = A.Fake<ICollection<MusicProviderOrderDTO>>();
            var MusicProviderOrderList = A.Fake<List<MusicProviderOrderDTO>>();
            A.CallTo(() => _mapper.Map<List<MusicProviderOrderDTO>>(MusicProviderOrders)).Returns(MusicProviderOrderList);
            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProviderOrders(); // ActionResult<APIResponse>
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull(); // Kontrollon që është kthyer 200 OK
            okResult!.Value.Should().BeOfType<APIResponse>(); // Kontrollon tipin e përmbajtjes
        }
        //GET MusicProviderOrder : 1. Invalid ID, 2. Valid ID, 3. Not found ID

        //1. Invalid ID unit test
        [Fact]
        public async Task GetMusicProviderOrder_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProviderOrder(invalidId);
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
        public async Task GetMusicProviderOrder_WithValidId_ReturnsOk()
        {
            // Arrange
            int MusicProviderOrderId = 1;
            var MusicProviderOrderEntity = new MusicProviderOrder { MusicProviderOrderId = MusicProviderOrderId }; // përdor instancë reale, jo Fake
            var MusicProviderOrderDto = new MusicProviderOrderDTO { MusicProviderOrderId = MusicProviderOrderId };       // e njëjta për DTO

            A.CallTo(() => _dbMusicProviderOrder.GetAsync(
                A<Expression<Func<MusicProviderOrder, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult(MusicProviderOrderEntity));

            A.CallTo(() => _mapper.Map<MusicProviderOrderDTO>(MusicProviderOrderEntity)).Returns(MusicProviderOrderDto);

            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProviderOrder(MusicProviderOrderId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(MusicProviderOrderDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //3. Not found ID unit test
        [Fact]
        public async Task GetMusicProviderOrder_NotFound_ReturnsNotFound()
        {
            // Arrange
            int MusicProviderOrderId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMusicProviderOrder.GetAsync(A<Expression<Func<MusicProviderOrder, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MusicProviderOrder>(null));


            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProviderOrder(MusicProviderOrderId);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }



        //CREATE MusicProviderOrder: 1. Valid Input , 2. Dublicate MusicProviderOrder Name, 3. Null MusicProviderOrderDTO

        //1. Valid Input unit test
        [Fact]
        public async Task CreateMusicProviderOrder_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new MusicProviderOrderDTO { OrderName = "Test MusicProviderOrder" };
            var createdEntity = new MusicProviderOrder { MusicProviderOrderId = 1, Name = "Test MusicProviderOrder" };
            var createdDto = new MusicProviderOrderDTO { MusicProviderOrderId = 1, OrderName = "Test MusicProviderOrder" };

            A.CallTo(() => _dbMusicProviderOrder.GetAsync(
                A<Expression<Func<MusicProviderOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<MusicProviderOrder>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<MusicProviderOrder>(createDto)).Returns(createdEntity);
            A.CallTo(() => _dbMusicProviderOrder.CreateAsync(createdEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<MusicProviderOrderDTO>(createdEntity)).Returns(createdDto);

            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMusicProviderOrder(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(createdDto);
        }
        //2. Dublicate MusicProviderOrder Name unit test
        [Fact]
        public async Task CreateMusicProviderOrder_DuplicateMusicProviderOrder_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new MusicProviderOrderDTO { OrderName = "Duplicate MusicProviderOrder" };
            var existingMusicProviderOrder = new MusicProviderOrder { MusicProviderOrderId = 1, Name = "Duplicate MusicProviderOrder" };

            A.CallTo(() => _dbMusicProviderOrder.GetAsync(
                A<Expression<Func<MusicProviderOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingMusicProviderOrder)); // Simulate duplicate

            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMusicProviderOrder(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        //3. Null MusicProviderOrderDTO unit test
        [Fact]
        public async Task CreateMusicProviderOrder_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMusicProviderOrder(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid Music Provider Order data.");
        }

        //DELETE MusicProviderOrder: 1. Invalid ID, 2. Null Entity, 3. Succesful Delete

        //Invalid ID unit testing
        [Fact]
        public async Task DeleteMusicProviderOrder_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.DeleteMusicProviderOrder(invalidId);
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
        public async Task DeleteMusicProviderOrder_NullInput_ReturnsBadRequest()
        {

            // Arrange
            int MusicProviderOrderId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMusicProviderOrder.GetAsync(A<Expression<Func<MusicProviderOrder, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MusicProviderOrder>(null));
            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMusicProviderOrder(MusicProviderOrderId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert

            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }

        //3. Succesful delete unit test
        [Fact]
        public async Task DeleteMusicProviderOrder_ValidId_ReturnsOkAndNoContentStatus()
        {
            // Arrange
            int MusicProviderOrderId = 1;
            var fakeMusicProviderOrder = new MusicProviderOrder
            {
                MusicProviderOrderId = MusicProviderOrderId,
                Name = "Sample MusicProviderOrder"
            };

            A.CallTo(() => _dbMusicProviderOrder.GetAsync(A<Expression<Func<MusicProviderOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(fakeMusicProviderOrder));

            A.CallTo(() => _dbMusicProviderOrder.DeleteMusicProviderOrderAsync(fakeMusicProviderOrder))
                .Returns(Task.CompletedTask);

            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMusicProviderOrder(MusicProviderOrderId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Also verify that DeleteMusicProviderOrderAsync was called
            A.CallTo(() => _dbMusicProviderOrder.DeleteMusicProviderOrderAsync(A<MusicProviderOrder>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task UpdateMusicProviderOrder_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int MusicProviderOrderId = 1;
            var updateDto = new MusicProviderOrderDTO { MusicProviderOrderId = MusicProviderOrderId, OrderName = "Updated MusicProviderOrder" };
            var mappedMusicProviderOrder = new MusicProviderOrder { MusicProviderOrderId = MusicProviderOrderId, Name = "Updated MusicProviderOrder" };

            A.CallTo(() => _mapper.Map<MusicProviderOrder>(updateDto)).Returns(mappedMusicProviderOrder);
            A.CallTo(() => _dbMusicProviderOrder.UpdateMusicProviderOrderAsync(A<MusicProviderOrder>.Ignored)).Returns(Task.FromResult(mappedMusicProviderOrder));

            var controller = new MusicProviderOrderController(_dbMusicProviderOrder, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateMusicProviderOrder(MusicProviderOrderId, updateDto);
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