using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
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
    public class PastryShopControllerTest
    {
        private readonly IPastryShopRepository _dbPastryShop;
        private readonly IMapper _mapper;

        public PastryShopControllerTest()
        {
            _dbPastryShop = A.Fake<IPastryShopRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task GetPastryShops_ReturnsOk()
        {
            var shops = new List<PastryShop>();
            var shopDtos = new List<PastryShopDTO>();

            A.CallTo(() => _dbPastryShop.GetAllPastryShopsAsync(null))
                .Returns(Task.FromResult<IEnumerable<PastryShop>>(shops));

            A.CallTo(() => _mapper.Map<List<PastryShopDTO>>(shops)).Returns(shopDtos);

            var controller = new PastryShopController(_dbPastryShop, _mapper);
            var result = await controller.GetPastryShops();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeOfType<APIResponse>();
        }

        [Fact]
        public async Task GetPastryShop_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryShopController(_dbPastryShop, _mapper);
            var result = await controller.GetPastryShop(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        //[Fact]
        //public async Task GetPastryShop_NotFound_ReturnsNotFound()
        //{
        //    A.CallTo(() => _dbPastryShop.GetPastryShopAsync(A<Expression<Func<PastryShop, bool>>>._))
        //        .Returns(Task.FromResult<PastryShop>(null));

        //    var controller = new PastryShopController(_dbPastryShop, _mapper);
        //    var result = await controller.GetPastryShop(100);
        //    result.Result.Should().BeOfType<NotFoundObjectResult>();
        //}

        //[Fact]
        //public async Task CreatePastryShop_Valid_ReturnsCreated()
        //{
        //    var dto = new PastryShopDTO { ShopName = "Sweet Treats" };
        //    var entity = new PastryShop { ShopId = 1, ShopName = "Sweet Treats" };

        //    A.CallTo(() => _dbPastryShop.GetPastryShopAsync(A<Expression<Func<PastryShop, bool>>>._))
        //        .Returns(Task.FromResult<PastryShop>(null));

        //    A.CallTo(() => _mapper.Map<PastryShop>(dto)).Returns(entity);
        //    A.CallTo(() => _dbPastryShop.CreatePastryShopAsync(entity)).Returns(Task.CompletedTask);
        //    A.CallTo(() => _mapper.Map<PastryShopDTO>(entity)).Returns(dto);

        //    var controller = new PastryShopController(_dbPastryShop, _mapper);
        //    var result = await controller.CreatePastryShop(dto);
        //    var created = result.Result as CreatedAtRouteResult;

        //    created.Should().NotBeNull();
        //    ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        //}

        [Fact]
        public async Task CreatePastryShop_Duplicate_ReturnsBadRequest()
        {
            var dto = new PastryShopDTO { ShopName = "Duplicate" };
            var entity = new PastryShop { ShopName = "Duplicate" };

            A.CallTo(() => _dbPastryShop.GetPastryShopAsync(A<Expression<Func<PastryShop, bool>>>._))
                .Returns(Task.FromResult(entity));

            var controller = new PastryShopController(_dbPastryShop, _mapper);
            var result = await controller.CreatePastryShop(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeletePastryShop_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryShopController(_dbPastryShop, _mapper);
            var result = await controller.DeletePastryShop(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        //[Fact]
        //public async Task DeletePastryShop_NotFound_ReturnsNotFound()
        //{
        //    A.CallTo(() => _dbPastryShop.GetPastryShopAsync(A<Expression<Func<PastryShop, bool>>>._))
        //        .Returns(Task.FromResult<PastryShop>(null));

        //    var controller = new PastryShopController(_dbPastryShop, _mapper);
        //    var result = await controller.DeletePastryShop(99);
        //    result.Result.Should().BeOfType<NotFoundObjectResult>();
        //}

        [Fact]
        public async Task DeletePastryShop_Success_ReturnsNoContent()
        {
            var shop = new PastryShop { ShopId = 1 };

            A.CallTo(() => _dbPastryShop.GetPastryShopAsync(A<Expression<Func<PastryShop, bool>>>._))
                .Returns(Task.FromResult(shop));

            A.CallTo(() => _dbPastryShop.DeletePastryShopAsync(shop)).Returns(Task.CompletedTask);

            var controller = new PastryShopController(_dbPastryShop, _mapper);
            var result = await controller.DeletePastryShop(1);
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePastryShop_ValidDto_ReturnsOk()
        {
            var dto = new PastryShopDTO { ShopId = 1, ShopName = "Updated Name" };
            var entity = new PastryShop { ShopId = 1, ShopName = "Updated Name" };

            A.CallTo(() => _mapper.Map<PastryShop>(dto)).Returns(entity);
            A.CallTo(() => _dbPastryShop.UpdatePastryShopAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new PastryShopController(_dbPastryShop, _mapper);
            var result = await controller.UpdatePastryShop(1, dto);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            var response = ok!.Value as APIResponse;
            response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
        }
    }
}
