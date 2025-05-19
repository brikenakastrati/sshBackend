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
    public class RestaurantControllerTest
    {
        private readonly IRestaurantRepository _dbRestaurant;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public RestaurantControllerTest()
        {
            _dbRestaurant = A.Fake<IRestaurantRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetRestaurants_ReturnsOk()
        {
            var list = new List<Restaurant>();
            var dtoList = new List<RestaurantDTO>();

            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored,
                A<Func<Task<IEnumerable<Restaurant>>>>.Ignored,
                A<TimeSpan>.Ignored)).Returns(Task.FromResult<IEnumerable<Restaurant>>(list));

            A.CallTo(() => _mapper.Map<List<RestaurantDTO>>(list)).Returns(dtoList);

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.GetRestaurants();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRestaurant_InvalidId_ReturnsBadRequest()
        {
            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.GetRestaurant(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetRestaurant_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbRestaurant.GetAsync(A<Expression<Func<Restaurant, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Restaurant>(null));

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.GetRestaurant(999);
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateRestaurant_Valid_ReturnsCreated()
        {
            var dto = new RestaurantDTO { RestaurantName = "My Place" };
            var entity = new Restaurant { RestaurantId = 1, RestaurantName = "My Place" };

            A.CallTo(() => _dbRestaurant.GetAsync(A<Expression<Func<Restaurant, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Restaurant>(null));

            A.CallTo(() => _mapper.Map<Restaurant>(dto)).Returns(entity);
            A.CallTo(() => _dbRestaurant.CreateAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<RestaurantDTO>(entity)).Returns(dto);

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.CreateRestaurant(dto);

            var created = result.Result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateRestaurant_Duplicate_ReturnsBadRequest()
        {
            var dto = new RestaurantDTO { RestaurantName = "Duplicate" };
            var entity = new Restaurant { RestaurantName = "Duplicate" };

            A.CallTo(() => _dbRestaurant.GetAsync(A<Expression<Func<Restaurant, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.CreateRestaurant(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteRestaurant_InvalidId_ReturnsBadRequest()
        {
            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.DeleteRestaurant(0);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteRestaurant_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbRestaurant.GetAsync(A<Expression<Func<Restaurant, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Restaurant>(null));

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.DeleteRestaurant(999);
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeleteRestaurant_Valid_ReturnsNoContent()
        {
            var entity = new Restaurant { RestaurantId = 1, RestaurantName = "ToDelete" };

            A.CallTo(() => _dbRestaurant.GetAsync(A<Expression<Func<Restaurant, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbRestaurant.DeleteRestaurantAsync(entity)).Returns(Task.CompletedTask);

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.DeleteRestaurant(1);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateRestaurant_Valid_ReturnsOk()
        {
            var dto = new RestaurantDTO { RestaurantId = 1, RestaurantName = "Updated" };
            var entity = new Restaurant { RestaurantId = 1, RestaurantName = "Updated" };

            A.CallTo(() => _mapper.Map<Restaurant>(dto)).Returns(entity);
            A.CallTo(() => _dbRestaurant.UpdateRestaurantAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.UpdateRestaurant(1, dto);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePartialRestaurant_Valid_ReturnsNoContent()
        {
            var entity = new Restaurant { RestaurantId = 1, RestaurantName = "Old" };
            var dto = new RestaurantDTO { RestaurantId = 1, RestaurantName = "Old" };

            A.CallTo(() => _dbRestaurant.GetAsync(A<Expression<Func<Restaurant, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<RestaurantDTO>(entity)).Returns(dto);
            A.CallTo(() => _mapper.Map<Restaurant>(dto)).Returns(entity);
            A.CallTo(() => _dbRestaurant.UpdateRestaurantAsync(entity)).Returns(Task.FromResult(entity));

            var patchDoc = new JsonPatchDocument<RestaurantDTO>();
            patchDoc.Replace(x => x.RestaurantName, "New");

            var controller = new RestaurantController(_dbRestaurant, _mapper, _cacheService);
            var result = await controller.UpdatePartialRestaurant(1, patchDoc);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
