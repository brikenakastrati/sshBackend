using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
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
    public class FlowerArrangementOrderControllerTest
    {
        private readonly IFlowerArrangementOrderRepository _dbOrderRepo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FlowerArrangementOrderControllerTest()
        {
            _dbOrderRepo = A.Fake<IFlowerArrangementOrderRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetFlowerArrangementOrders_ReturnsOk()
        {
            var orders = A.Fake<ICollection<FlowerArrangementOrder>>();
            var orderDTOs = A.Fake<List<FlowerArrangementOrderDTO>>();
            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>._, A<Func<Task<IEnumerable<FlowerArrangementOrder>>>>._, A<TimeSpan>._))
                .Returns(Task.FromResult<IEnumerable<FlowerArrangementOrder>>(orders));
            A.CallTo(() => _mapper.Map<List<FlowerArrangementOrderDTO>>(orders)).Returns(orderDTOs);

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);

            var result = await controller.GetFlowerArrangementOrders();
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeOfType<APIResponse>();
        }

        [Fact]
        public async Task GetFlowerArrangementOrder_InvalidId_ReturnsBadRequest()
        {
            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);

            var result = await controller.GetFlowerArrangementOrder(0);
            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetFlowerArrangementOrder_ValidId_ReturnsOk()
        {
            var entity = new FlowerArrangementOrder { FlowerArrangementOrderId = 1, OrderName = "Sample Order" };
            var dto = new FlowerArrangementOrderDTO { FlowerArrangementOrderId = 1, OrderName = "Sample Order" };

            A.CallTo(() => _dbOrderRepo.GetAsync(A<Expression<Func<FlowerArrangementOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<FlowerArrangementOrderDTO>(entity)).Returns(dto);

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);
            var result = await controller.GetFlowerArrangementOrder(1);
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).Result.Should().Be(dto);
        }

        [Fact]
        public async Task GetFlowerArrangementOrder_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbOrderRepo.GetAsync(A<Expression<Func<FlowerArrangementOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangementOrder>(null));

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);
            var result = await controller.GetFlowerArrangementOrder(99);
            var notFoundResult = result.Result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateFlowerArrangementOrder_Valid_ReturnsCreated()
        {
            var dto = new FlowerArrangementOrderDTO { OrderName = "New Order" };
            var entity = new FlowerArrangementOrder { FlowerArrangementOrderId = 1, OrderName = "New Order" };

            A.CallTo(() => _dbOrderRepo.GetAsync(A<Expression<Func<FlowerArrangementOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangementOrder>(null));
            A.CallTo(() => _mapper.Map<FlowerArrangementOrder>(dto)).Returns(entity);
            A.CallTo(() => _dbOrderRepo.CreateAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<FlowerArrangementOrderDTO>(entity)).Returns(dto);

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);
            var result = await controller.CreateFlowerArrangementOrder(dto);
            var createdResult = result.Result as CreatedAtRouteResult;

            createdResult.Should().NotBeNull();
            ((APIResponse)createdResult!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateFlowerArrangementOrder_Duplicate_ReturnsBadRequest()
        {
            var dto = new FlowerArrangementOrderDTO { OrderName = "Existing Order" };
            var entity = new FlowerArrangementOrder { OrderName = "Existing Order" };

            A.CallTo(() => _dbOrderRepo.GetAsync(A<Expression<Func<FlowerArrangementOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);
            var result = await controller.CreateFlowerArrangementOrder(dto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFlowerArrangementOrder_InvalidId_ReturnsBadRequest()
        {
            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);
            var result = await controller.DeleteFlowerArrangementOrder(-1);
            var badRequestResult = result.Result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateFlowerArrangementOrder_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int FlowerArrangementOrderId = 1;
            var updateDto = new FlowerArrangementOrderDTO { FlowerArrangementOrderId = FlowerArrangementOrderId, OrderName = "Updated FlowerArrangementOrder" };
            var mappedFlowerArrangementOrder = new FlowerArrangementOrder { FlowerArrangementOrderId = FlowerArrangementOrderId, OrderName = "Updated FlowerArrangementOrder" };

            A.CallTo(() => _mapper.Map<FlowerArrangementOrder>(updateDto)).Returns(mappedFlowerArrangementOrder);
            A.CallTo(() => _dbOrderRepo.UpdateFlowerArrangementOrderAsync(A<FlowerArrangementOrder>.Ignored)).Returns(Task.FromResult(mappedFlowerArrangementOrder)); // Change here

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService); // Also change here

            // Act
            var result = await controller.UpdateFlowerArrangementOrder(FlowerArrangementOrderId, updateDto);
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
        public async Task DeleteFlowerArrangementOrder_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbOrderRepo.GetAsync(A<Expression<Func<FlowerArrangementOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<FlowerArrangementOrder>(null));

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);
            var result = await controller.DeleteFlowerArrangementOrder(10);
            var notFoundResult = result.Result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFlowerArrangementOrder_Successful_ReturnsNoContent()
        {
            var entity = new FlowerArrangementOrder { FlowerArrangementOrderId = 1 };

            A.CallTo(() => _dbOrderRepo.GetAsync(A<Expression<Func<FlowerArrangementOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbOrderRepo.DeleteFlowerArrangementOrderAsync(entity)).Returns(Task.CompletedTask);

            var controller = new FlowerArrangementOrderController(_dbOrderRepo, _mapper, _cacheService);
            var result = await controller.DeleteFlowerArrangementOrder(1);
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

       
    }
}
