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
    public class OrderStatusControllerTest
    {
        private readonly IOrderStatusRepository _dbOrderStatus;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public OrderStatusControllerTest()
        {
            _dbOrderStatus = A.Fake<IOrderStatusRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]

        //GET ALL OrderStatusS UNIT TEST
        public async Task OrderStatusController_GetOrderStatuss_ReturnOk()

        {
            var testId = 1;
            // Arrange
            var OrderStatuss = A.Fake<ICollection<OrderStatusDTO>>();
            var OrderStatusList = A.Fake<List<OrderStatusDTO>>();
            A.CallTo(() => _mapper.Map<List<OrderStatusDTO>>(OrderStatuss)).Returns(OrderStatusList);
            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetOrderStatus(testId); // ActionResult<APIResponse>
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull(); // Kontrollon që është kthyer 200 OK
            okResult!.Value.Should().BeOfType<APIResponse>(); // Kontrollon tipin e përmbajtjes
        }
        //GET OrderStatus : 1. Invalid ID, 2. Valid ID, 3. Not found ID

        //1. Invalid ID unit test
        [Fact]
        public async Task GetOrderStatus_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetOrderStatus(invalidId);
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
        public async Task GetOrderStatus_WithValidId_ReturnsOk()
        {
            // Arrange
            int OrderStatusId = 1;
            var OrderStatusEntity = new OrderStatus { OrderStatusId = OrderStatusId }; // përdor instancë reale, jo Fake
            var OrderStatusDto = new OrderStatusDTO { OrderStatusId = OrderStatusId };       // e njëjta për DTO

            A.CallTo(() => _dbOrderStatus.GetAsync(
                A<Expression<Func<OrderStatus, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult(OrderStatusEntity));

            A.CallTo(() => _mapper.Map<OrderStatusDTO>(OrderStatusEntity)).Returns(OrderStatusDto);

            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetOrderStatus(OrderStatusId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(OrderStatusDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //3. Not found ID unit test
        [Fact]
        public async Task GetOrderStatus_NotFound_ReturnsNotFound()
        {
            // Arrange
            int OrderStatusId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbOrderStatus.GetAsync(A<Expression<Func<OrderStatus, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<OrderStatus>(null));


            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetOrderStatus(OrderStatusId);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }



        //CREATE OrderStatus: 1. Valid Input , 2. Dublicate OrderStatus Name, 3. Null OrderStatusDTO

        //1. Valid Input unit test
        [Fact]
        public async Task CreateOrderStatus_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new OrderStatusDTO { OrderStatusName = "Test OrderStatus" };
            var createdEntity = new OrderStatus { OrderStatusId = 1, OrderStatusName = "Test OrderStatus" };
            var createdDto = new OrderStatusDTO { OrderStatusId = 1, OrderStatusName = "Test OrderStatus" };

            A.CallTo(() => _dbOrderStatus.GetAsync(
                A<Expression<Func<OrderStatus, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<OrderStatus>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<OrderStatus>(createDto)).Returns(createdEntity);
            A.CallTo(() => _dbOrderStatus.CreateAsync(createdEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<OrderStatusDTO>(createdEntity)).Returns(createdDto);

            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var result = await controller.CreateOrderStatus(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(createdDto);
        }
        //2. Dublicate OrderStatus Name unit test
        [Fact]
        public async Task CreateOrderStatus_DuplicateOrderStatus_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new OrderStatusDTO { OrderStatusName = "Duplicate OrderStatus" };
            var existingOrderStatus = new OrderStatus { OrderStatusId = 1, OrderStatusName = "Duplicate OrderStatus" };

            A.CallTo(() => _dbOrderStatus.GetAsync(
                A<Expression<Func<OrderStatus, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingOrderStatus)); // Simulate duplicate

            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var result = await controller.CreateOrderStatus(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        //3. Null OrderStatusDTO unit test
        [Fact]
        public async Task CreateOrderStatus_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var result = await controller.CreateOrderStatus(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid Order Status data.");
        }

        //DELETE OrderStatus: 1. Invalid ID, 2. Null Entity, 3. Succesful Delete

        //Invalid ID unit testing
        [Fact]
        public async Task DeleteOrderStatus_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var actionResult = await controller.DeleteOrderStatus(invalidId);
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
        public async Task DeleteOrderStatus_NullInput_ReturnsBadRequest()
        {

            // Arrange
            int OrderStatusId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbOrderStatus.GetAsync(A<Expression<Func<OrderStatus, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<OrderStatus>(null));
            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteOrderStatus(OrderStatusId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert

            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }

        //3. Succesful delete unit test
        [Fact]
        public async Task DeleteOrderStatus_ValidId_ReturnsOkAndNoContentStatus()
        {
            // Arrange
            int OrderStatusId = 1;
            var fakeOrderStatus = new OrderStatus
            {
                OrderStatusId = OrderStatusId,
                OrderStatusName = "Sample OrderStatus"
            };

            A.CallTo(() => _dbOrderStatus.GetAsync(A<Expression<Func<OrderStatus, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(fakeOrderStatus));

            A.CallTo(() => _dbOrderStatus.DeleteOrderStatusAsync(fakeOrderStatus))
                .Returns(Task.CompletedTask);

            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteOrderStatus(OrderStatusId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Also verify that DeleteOrderStatusAsync was called
            A.CallTo(() => _dbOrderStatus.DeleteOrderStatusAsync(A<OrderStatus>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task UpdateOrderStatus_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int OrderStatusId = 1;
            var updateDto = new OrderStatusDTO { OrderStatusId = OrderStatusId, OrderStatusName = "Updated OrderStatus" };
            var mappedOrderStatus = new OrderStatus { OrderStatusId = OrderStatusId, OrderStatusName = "Updated OrderStatus" };

            A.CallTo(() => _mapper.Map<OrderStatus>(updateDto)).Returns(mappedOrderStatus);
            A.CallTo(() => _dbOrderStatus.UpdateOrderStatusAsync(A<OrderStatus>.Ignored)).Returns(Task.FromResult(mappedOrderStatus));

            var controller = new OrderStatusController(_dbOrderStatus, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateOrderStatus(OrderStatusId, updateDto);
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