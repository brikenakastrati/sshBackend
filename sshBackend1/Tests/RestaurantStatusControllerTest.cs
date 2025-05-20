//using AutoMapper;
//using FakeItEasy;
//using FluentAssertions;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using sshBackend1.Controllers;
//using sshBackend1.Models;
//using sshBackend1.Models.DTOs;
//using sshBackend1.Repository.IRepository;
//using sshBackend1.Services.IServices;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;

//namespace UnitTests.ControllerTests
//{
//    public class RestaurantStatusControllerTest
//    {
//        private readonly IRestaurantStatusRepository _dbRestaurantStatus;
//        private readonly IMapper _mapper;
//        private readonly ICacheService _cacheService;

//        public RestaurantStatusControllerTest()
//        {
//            _dbRestaurantStatus = A.Fake<IRestaurantStatusRepository>();
//            _mapper = A.Fake<IMapper>();
//            _cacheService = A.Fake<ICacheService>();
//        }

//        [Fact]
//        public async Task GetRestaurantStatuses_ReturnsOk()
//        {
//            var entityList = new List<RestaurantStatus>();
//            var dtoList = new List<RestaurantStatusDTO>();

//            A.CallTo(() => _cacheService.GetOrAddAsync(
//                A<string>.Ignored,
//                A<Func<Task<IEnumerable<RestaurantStatus>>>>.Ignored,
//                A<TimeSpan>.Ignored)).Returns(Task.FromResult<IEnumerable<RestaurantStatus>>(entityList));

//            A.CallTo(() => _mapper.Map<List<RestaurantStatusDTO>>(entityList)).Returns(dtoList);

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.GetRestaurantStatuses();

//            var okResult = result.Result as OkObjectResult;
//            okResult.Should().NotBeNull();
//            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        [Fact]
//        public async Task GetRestaurantStatus_InvalidId_ReturnsBadRequest()
//        {
//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.GetRestaurantStatus(-1);
//            result.Result.Should().BeOfType<BadRequestObjectResult>();
//        }

//        [Fact]
//        public async Task GetRestaurantStatus_NotFound_ReturnsNotFound()
//        {
//            A.CallTo(() => _dbRestaurantStatus.GetAsync(A<Expression<Func<RestaurantStatus, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult<RestaurantStatus>(null));

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.GetRestaurantStatus(999);

//            result.Result.Should().BeOfType<NotFoundObjectResult>();
//        }

//        [Fact]
//        public async Task CreateRestaurantStatus_Valid_ReturnsCreated()
//        {
//            var dto = new RestaurantStatusDTO { Name = "Open" };
//            var entity = new RestaurantStatus { RestaurantStatusId = 1, Name = "Open" };

//            A.CallTo(() => _dbRestaurantStatus.GetAsync(A<Expression<Func<RestaurantStatus, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult<RestaurantStatus>(null));
//            A.CallTo(() => _mapper.Map<RestaurantStatus>(dto)).Returns(entity);
//            A.CallTo(() => _dbRestaurantStatus.CreateAsync(entity)).Returns(Task.CompletedTask);
//            A.CallTo(() => _mapper.Map<RestaurantStatusDTO>(entity)).Returns(dto);

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.CreateRestaurantStatus(dto);

//            var created = result.Result as CreatedAtRouteResult;
//            created.Should().NotBeNull();
//            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
//        }

//        [Fact]
//        public async Task CreateRestaurantStatus_Duplicate_ReturnsBadRequest()
//        {
//            var dto = new RestaurantStatusDTO { Name = "Duplicate" };
//            var entity = new RestaurantStatus { Name = "Duplicate" };

//            A.CallTo(() => _dbRestaurantStatus.GetAsync(A<Expression<Func<RestaurantStatus, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult(entity));

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.CreateRestaurantStatus(dto);

//            result.Result.Should().BeOfType<BadRequestObjectResult>();
//        }

//        [Fact]
//        public async Task DeleteRestaurantStatus_InvalidId_ReturnsBadRequest()
//        {
//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.DeleteRestaurantStatus(0);
//            result.Result.Should().BeOfType<BadRequestObjectResult>();
//        }

//        [Fact]
//        public async Task DeleteRestaurantStatus_NotFound_ReturnsNotFound()
//        {
//            A.CallTo(() => _dbRestaurantStatus.GetAsync(A<Expression<Func<RestaurantStatus, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult<RestaurantStatus>(null));

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.DeleteRestaurantStatus(999);
//            result.Result.Should().BeOfType<NotFoundObjectResult>();
//        }

//        [Fact]
//        public async Task DeleteRestaurantStatus_Valid_ReturnsNoContent()
//        {
//            var entity = new RestaurantStatus { RestaurantStatusId = 1, Name = "To Delete" };

//            A.CallTo(() => _dbRestaurantStatus.GetAsync(A<Expression<Func<RestaurantStatus, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult(entity));
//            A.CallTo(() => _dbRestaurantStatus.DeleteRestaurantStatusAsync(entity)).Returns(Task.CompletedTask);

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.DeleteRestaurantStatus(1);
//            var ok = result.Result as OkObjectResult;

//            ok.Should().NotBeNull();
//            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }

//        [Fact]
//        public async Task UpdateRestaurantStatus_Valid_ReturnsOk()
//        {
//            var dto = new RestaurantStatusDTO { RestaurantStatusId = 1, Name = "Updated" };
//            var entity = new RestaurantStatus { RestaurantStatusId = 1, Name = "Updated" };

//            A.CallTo(() => _mapper.Map<RestaurantStatus>(dto)).Returns(entity);
//            A.CallTo(() => _dbRestaurantStatus.UpdateRestaurantStatusAsync(entity)).Returns(Task.FromResult(entity));

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.UpdateRestaurantStatus(1, dto);
//            var ok = result.Result as OkObjectResult;

//            ok.Should().NotBeNull();
//            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }

//        [Fact]
//        public async Task UpdatePartialRestaurantStatus_Valid_ReturnsNoContent()
//        {
//            var entity = new RestaurantStatus { RestaurantStatusId = 1, Name = "Old" };
//            var dto = new RestaurantStatusDTO { RestaurantStatusId = 1, Name = "Old" };

//            A.CallTo(() => _dbRestaurantStatus.GetAsync(A<Expression<Func<RestaurantStatus, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult(entity));
//            A.CallTo(() => _mapper.Map<RestaurantStatusDTO>(entity)).Returns(dto);
//            A.CallTo(() => _mapper.Map<RestaurantStatus>(dto)).Returns(entity);
//            A.CallTo(() => _dbRestaurantStatus.UpdateRestaurantStatusAsync(entity)).Returns(Task.FromResult(entity));

//            var patchDoc = new JsonPatchDocument<RestaurantStatusDTO>();
//            patchDoc.Replace(x => x.Name, "New");

//            var controller = new RestaurantStatusController(_dbRestaurantStatus, _mapper, _cacheService);
//            var result = await controller.UpdatePartialRestaurantStatus(1, patchDoc);

//            result.Should().BeOfType<NoContentResult>();
//        }
//    }
//}
