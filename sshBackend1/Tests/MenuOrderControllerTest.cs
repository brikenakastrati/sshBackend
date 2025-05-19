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
    public class MenuOrderControllerTest
    {
        private readonly IMenuOrderRepository _dbMenuOrder;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public MenuOrderControllerTest()
        {
            _dbMenuOrder = A.Fake<IMenuOrderRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]

        //GET ALL MenuOrderS UNIT TEST
        public async Task MenuOrderController_GetMenuOrders_ReturnOk()
        {
            // Arrange
            var MenuOrders = A.Fake<ICollection<MenuOrderDTO>>();
            var MenuOrderList = A.Fake<List<MenuOrderDTO>>();
            A.CallTo(() => _mapper.Map<List<MenuOrderDTO>>(MenuOrders)).Returns(MenuOrderList);
            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuOrders(); // ActionResult<APIResponse>
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull(); // Kontrollon që është kthyer 200 OK
            okResult!.Value.Should().BeOfType<APIResponse>(); // Kontrollon tipin e përmbajtjes
        }
        //GET MenuOrder : 1. Invalid ID, 2. Valid ID, 3. Not found ID

        //1. Invalid ID unit test
        [Fact]
        public async Task GetMenuOrder_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuOrder(invalidId);
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
        public async Task GetMenuOrder_WithValidId_ReturnsOk()
        {
            // Arrange
            int MenuOrderId = 1;
            var MenuOrderEntity = new MenuOrder { MenuOrderId = MenuOrderId }; // përdor instancë reale, jo Fake
            var MenuOrderDto = new MenuOrderDTO { MenuOrderId = MenuOrderId };       // e njëjta për DTO

            A.CallTo(() => _dbMenuOrder.GetAsync(
                A<Expression<Func<MenuOrder, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult(MenuOrderEntity));

            A.CallTo(() => _mapper.Map<MenuOrderDTO>(MenuOrderEntity)).Returns(MenuOrderDto);

            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuOrder(MenuOrderId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(MenuOrderDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //3. Not found ID unit test
        [Fact]
        public async Task GetMenuOrder_NotFound_ReturnsNotFound()
        {
            // Arrange
            int MenuOrderId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMenuOrder.GetAsync(A<Expression<Func<MenuOrder, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MenuOrder>(null));


            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuOrder(MenuOrderId);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }



        //CREATE MenuOrder: 1. Valid Input , 2. Dublicate MenuOrder Name, 3. Null MenuOrderDTO

        //1. Valid Input unit test
        [Fact]
        public async Task CreateMenuOrder_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new MenuOrderDTO { OrderName = "Test MenuOrder" };
            var createdEntity = new MenuOrder { MenuOrderId = 1, OrderName = "Test MenuOrder" };
            var createdDto = new MenuOrderDTO { MenuOrderId = 1, OrderName = "Test MenuOrder" };

            A.CallTo(() => _dbMenuOrder.GetAsync(
                A<Expression<Func<MenuOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<MenuOrder>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<MenuOrder>(createDto)).Returns(createdEntity);
            A.CallTo(() => _dbMenuOrder.CreateAsync(createdEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<MenuOrderDTO>(createdEntity)).Returns(createdDto);

            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenuOrder(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(createdDto);
        }
        //2. Dublicate MenuOrder Name unit test
        [Fact]
        public async Task CreateMenuOrder_DuplicateMenuOrder_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new MenuOrderDTO { OrderName = "Duplicate MenuOrder" };
            var existingMenuOrder = new MenuOrder { MenuOrderId = 1, OrderName = "Duplicate MenuOrder" };

            A.CallTo(() => _dbMenuOrder.GetAsync(
                A<Expression<Func<MenuOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingMenuOrder)); // Simulate duplicate

            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenuOrder(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        //3. Null MenuOrderDTO unit test
        [Fact]
        public async Task CreateMenuOrder_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenuOrder(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid Menu Order data.");
        }

        //DELETE MenuOrder: 1. Invalid ID, 2. Null Entity, 3. Succesful Delete

        //Invalid ID unit testing
        [Fact]
        public async Task DeleteMenuOrder_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var actionResult = await controller.DeleteMenuOrder(invalidId);
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
        public async Task DeleteMenuOrder_NullInput_ReturnsBadRequest()
        {

            // Arrange
            int MenuOrderId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMenuOrder.GetAsync(A<Expression<Func<MenuOrder, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MenuOrder>(null));
            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMenuOrder(MenuOrderId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert

            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }

        //3. Succesful delete unit test
        [Fact]
        public async Task DeleteMenuOrder_ValidId_ReturnsOkAndNoContentStatus()
        {
            // Arrange
            int MenuOrderId = 1;
            var fakeMenuOrder = new MenuOrder
            {
                MenuOrderId = MenuOrderId,
                OrderName = "Sample MenuOrder"
            };

            A.CallTo(() => _dbMenuOrder.GetAsync(A<Expression<Func<MenuOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(fakeMenuOrder));

            A.CallTo(() => _dbMenuOrder.DeleteMenuOrderAsync(fakeMenuOrder))
                .Returns(Task.CompletedTask);

            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMenuOrder(MenuOrderId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Also verify that DeleteMenuOrderAsync was called
            A.CallTo(() => _dbMenuOrder.DeleteMenuOrderAsync(A<MenuOrder>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task UpdateMenuOrder_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int MenuOrderId = 1;
            var updateDto = new MenuOrderDTO { MenuOrderId = MenuOrderId, OrderName = "Updated MenuOrder" };
            var mappedMenuOrder = new MenuOrder { MenuOrderId = MenuOrderId, OrderName = "Updated MenuOrder" };

            A.CallTo(() => _mapper.Map<MenuOrder>(updateDto)).Returns(mappedMenuOrder);
            A.CallTo(() => _dbMenuOrder.UpdateMenuOrderAsync(A<MenuOrder>.Ignored)).Returns(Task.FromResult(mappedMenuOrder));

            var controller = new MenuOrderController(_dbMenuOrder, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateMenuOrder(MenuOrderId, updateDto);
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