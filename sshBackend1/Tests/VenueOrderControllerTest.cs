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
    public class VenueOrderControllerTest
    {
        private readonly IVenueOrderRepository _dbVenueOrder;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public VenueOrderControllerTest()
        {
            _dbVenueOrder = A.Fake<IVenueOrderRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetEvents_ReturnsOk()
        {
            var entities = new List<VenueOrder>();
            var dtos = new List<VenueOrderDTO>();

            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored,
                A<Func<Task<IEnumerable<VenueOrder>>>>.Ignored,
                A<TimeSpan>.Ignored)).Returns(Task.FromResult<IEnumerable<VenueOrder>>(entities));

            A.CallTo(() => _mapper.Map<List<VenueOrderDTO>>(entities)).Returns(dtos);

            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.GetEvents();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetVenueOrder_InvalidId_ReturnsBadRequest()
        {
            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.GetVenueOrder(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetVenueOrder_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbVenueOrder.GetAsync(A<Expression<Func<VenueOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<VenueOrder>(null));

            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.GetVenueOrder(123);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateEvent_Valid_ReturnsCreated()
        {
            var dto = new VenueOrderDTO { Name = "New Order" };
            var entity = new VenueOrder { VenueOrderId = 1, Name = "New Order" };

            A.CallTo(() => _dbVenueOrder.GetAsync(A<Expression<Func<VenueOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<VenueOrder>(null));

            A.CallTo(() => _mapper.Map<VenueOrder>(dto)).Returns(entity);
            A.CallTo(() => _dbVenueOrder.CreateAsync(entity)).Returns(Task.CompletedTask);

            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.CreateEvent(dto);

            var created = result.Result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateEvent_Duplicate_ReturnsBadRequest()
        {
            var dto = new VenueOrderDTO { Name = "Existing" };
            var entity = new VenueOrder { Name = "Existing" };

            A.CallTo(() => _dbVenueOrder.GetAsync(A<Expression<Func<VenueOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.CreateEvent(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteVenueOrder_Valid_ReturnsOk()
        {
            var entity = new VenueOrder { VenueOrderId = 1 };

            A.CallTo(() => _dbVenueOrder.GetAsync(A<Expression<Func<VenueOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbVenueOrder.DeleteVenueOrderAsync(entity)).Returns(Task.CompletedTask);

            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.DeleteVenueOrder(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateVenueOrder_Valid_ReturnsOk()
        {
            var dto = new VenueOrderDTO { VenueOrderId = 1, Name = "Updated" };
            var entity = new VenueOrder { VenueOrderId = 1, Name = "Updated" };

            A.CallTo(() => _mapper.Map<VenueOrder>(dto)).Returns(entity);
            A.CallTo(() => _dbVenueOrder.UpdateVenueOrderAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.UpdateVenueOrder(1, dto);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePartialVenueOrder_Valid_ReturnsNoContent()
        {
            var entity = new VenueOrder { VenueOrderId = 1, Name = "Old Name" };
            var dto = new VenueOrderDTO { VenueOrderId = 1, Name = "Old Name" };

            A.CallTo(() => _dbVenueOrder.GetAsync(A<Expression<Func<VenueOrder, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<VenueOrderDTO>(entity)).Returns(dto);
            A.CallTo(() => _mapper.Map<VenueOrder>(dto)).Returns(entity);

            var patchDoc = new JsonPatchDocument<VenueOrderDTO>();
            patchDoc.Replace(x => x.Name, "New Name");

            var controller = new VenueOrderController(_dbVenueOrder, _mapper, _cacheService);
            var result = await controller.UpdatePartialVenueOrder(1, patchDoc);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
