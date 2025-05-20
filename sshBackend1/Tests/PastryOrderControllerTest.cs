using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Controllers;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class PastryOrderControllerTest
    {
        private readonly IPastryOrderRepository _dbPastryOrder;
        private readonly IMapper _mapper;

        public PastryOrderControllerTest()
        {
            _dbPastryOrder = A.Fake<IPastryOrderRepository>();
            _mapper = A.Fake<IMapper>();
        }

        //[Fact]
        //public async Task GetPastryOrders_ReturnsOk()
        //{
        //    var orders = new List<PastryOrder>
        //    {
        //        new PastryOrder { PastryOrderId = 1, OrderName = "Order A" },
        //        new PastryOrder { PastryOrderId = 2, OrderName = "Order B" }
        //    };

        //    var dtos = new List<PastryOrderDTO>
        //    {
        //        new PastryOrderDTO { PastryOrderId = 1, OrderName = "Order A" },
        //        new PastryOrderDTO { PastryOrderId = 2, OrderName = "Order B" }
        //    };

        //    A.CallTo(() => _dbPastryOrder.GetAllPastryOrdersAsync(null)).Returns(Task.FromResult((IEnumerable<PastryOrder>)orders));
        //    A.CallTo(() => _mapper.Map<List<PastryOrderDTO>>(orders)).Returns(dtos);

        //    var controller = new PastryOrderController(_dbPastryOrder, _mapper);
        //    var result = await controller.GetPastryOrders();

        //    var ok = result.Result as OkObjectResult;
        //    ok.Should().NotBeNull();

        //    var response = ok!.Value as APIResponse;
        //    response.Should().NotBeNull();
        //    response!.Result.Should().BeEquivalentTo(dtos);
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}

        [Fact]
        public async Task GetPastryOrder_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryOrderController(_dbPastryOrder, _mapper);
            var result = await controller.GetPastryOrder(-1);

            var bad = result.Result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            ((APIResponse)bad!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //[Fact]
        //public async Task GetPastryOrder_NotFound_ReturnsNotFound()
        //{
        //    A.CallTo(() => _dbPastryOrder.GetPastryOrderAsync(A<Expression<Func<PastryOrder, bool>>>._)).Returns(Task.FromResult<PastryOrder>(null));

        //    var controller = new PastryOrderController(_dbPastryOrder, _mapper);
        //    var result = await controller.GetPastryOrder(100);

        //    var notFound = result.Result as NotFoundObjectResult;
        //    notFound.Should().NotBeNull();
        //    ((APIResponse)notFound!.Value!).StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async Task GetPastryOrder_ValidId_ReturnsOk()
        //{
        //    var entity = new PastryOrder { PastryOrderId = 1, OrderName = "Test" };
        //    var dto = new PastryOrderDTO { PastryOrderId = 1, OrderName = "Test" };

        //    A.CallTo(() => _dbPastryOrder.GetPastryOrderAsync(A<Expression<Func<PastryOrder, bool>>>._)).Returns(Task.FromResult(entity));
        //    A.CallTo(() => _mapper.Map<PastryOrderDTO>(entity)).Returns(dto);

        //    var controller = new PastryOrderController(_dbPastryOrder, _mapper);
        //    var result = await controller.GetPastryOrder(1);

        //    var ok = result.Result as OkObjectResult;
        //    ok.Should().NotBeNull();
        //    ((APIResponse)ok!.Value!).Result.Should().BeEquivalentTo(dto);
        //}

        [Fact]
        public async Task CreatePastryOrder_NullDto_ReturnsBadRequest()
        {
            var controller = new PastryOrderController(_dbPastryOrder, _mapper);
            var result = await controller.CreatePastryOrder(null);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Invalid pastry order data.");
        }

        [Fact]
        public async Task CreatePastryOrder_DuplicateOrderName_ReturnsBadRequest()
        {
            var dto = new PastryOrderDTO { OrderName = "Duplicate" };
            var existing = new PastryOrder { OrderName = "Duplicate" };

            A.CallTo(() => _dbPastryOrder.GetPastryOrderAsync(A<Expression<Func<PastryOrder, bool>>>._)).Returns(Task.FromResult(existing));

            var controller = new PastryOrderController(_dbPastryOrder, _mapper);
            var result = await controller.CreatePastryOrder(dto);

            var bad = result.Result as BadRequestObjectResult;
            bad.Should().NotBeNull();
        }

        //[Fact]
        //public async Task CreatePastryOrder_ValidDto_ReturnsCreatedAtRoute()
        //{
        //    var dto = new PastryOrderDTO { OrderName = "New" };
        //    var entity = new PastryOrder { PastryOrderId = 1, OrderName = "New" };

        //    A.CallTo(() => _dbPastryOrder.GetPastryOrderAsync(A<Expression<Func<PastryOrder, bool>>>._)).Returns(Task.FromResult<PastryOrder>(null));
        //    A.CallTo(() => _mapper.Map<PastryOrder>(dto)).Returns(entity);
        //    A.CallTo(() => _dbPastryOrder.CreatePastryOrderAsync(entity)).Returns(Task.CompletedTask);
        //    A.CallTo(() => _mapper.Map<PastryOrderDTO>(entity)).Returns(new PastryOrderDTO { PastryOrderId = 1, OrderName = "New" });

        //    var controller = new PastryOrderController(_dbPastryOrder, _mapper);
        //    var result = await controller.CreatePastryOrder(dto);

        //    var created = result.Result as CreatedAtRouteResult;
        //    created.Should().NotBeNull();
        //    ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        //}

        [Fact]
        public async Task DeletePastryOrder_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryOrderController(_dbPastryOrder, _mapper);
            var result = await controller.DeletePastryOrder(-1);

            var bad = result.Result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            ((APIResponse)bad!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //[Fact]
        //public async Task DeletePastryOrder_NotFound_ReturnsNotFound()
        //{
        //    A.CallTo(() => _dbPastryOrder.GetPastryOrderAsync(A<Expression<Func<PastryOrder, bool>>>._)).Returns(Task.FromResult<PastryOrder>(null));

        //    var controller = new PastryOrderController(_dbPastryOrder, _mapper);
        //    var result = await controller.DeletePastryOrder(200);

        //    var notFound = result.Result as NotFoundObjectResult;
        //    notFound.Should().NotBeNull();
        //    ((APIResponse)notFound!.Value!).StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}

        [Fact]
        public async Task DeletePastryOrder_ValidId_ReturnsOk()
        {
            var entity = new PastryOrder { PastryOrderId = 1, OrderName = "DeleteMe" };

            A.CallTo(() => _dbPastryOrder.GetPastryOrderAsync(A<Expression<Func<PastryOrder, bool>>>._)).Returns(Task.FromResult(entity));
            A.CallTo(() => _dbPastryOrder.DeletePastryOrderAsync(entity)).Returns(Task.CompletedTask);

            var controller = new PastryOrderController(_dbPastryOrder, _mapper);
            var result = await controller.DeletePastryOrder(1);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePastryOrder_ValidDto_ReturnsOk()
        {
            var dto = new PastryOrderDTO { PastryOrderId = 1, OrderName = "Updated" };
            var model = new PastryOrder { PastryOrderId = 1, OrderName = "Updated" };

            A.CallTo(() => _mapper.Map<PastryOrder>(dto)).Returns(model);
            A.CallTo(() => _dbPastryOrder.UpdatePastryOrderAsync(model)).Returns(Task.FromResult(model));

            var controller = new PastryOrderController(_dbPastryOrder, _mapper);
            var result = await controller.UpdatePastryOrder(1, dto);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
