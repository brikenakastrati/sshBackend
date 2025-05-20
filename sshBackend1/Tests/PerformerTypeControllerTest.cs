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
    public class PerformerTypeControllerTest
    {
        private readonly IPerformerTypeRepository _dbPerformerType;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public PerformerTypeControllerTest()
        {
            _dbPerformerType = A.Fake<IPerformerTypeRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetPerformerTypes_ReturnsOk()
        {
            var list = new List<PerformerType>();
            var dtoList = new List<PerformerTypeDTO>();

            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored,
                A<Func<Task<IEnumerable<PerformerType>>>>.Ignored,
                A<TimeSpan>.Ignored)).Returns(Task.FromResult<IEnumerable<PerformerType>>(list));

            A.CallTo(() => _mapper.Map<List<PerformerTypeDTO>>(list)).Returns(dtoList);

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.GetPerformerTypes();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPerformerType_InvalidId_ReturnsBadRequest()
        {
            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.GetPerformerType(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetPerformerType_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPerformerType.GetAsync(A<Expression<Func<PerformerType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<PerformerType>(null));

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.GetPerformerType(999);
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreatePerformerType_Valid_ReturnsCreated()
        {
            var dto = new PerformerTypeDTO { Name = "Musician" };
            var entity = new PerformerType { PerformerTypeId = 1, Name = "Musician" };

            A.CallTo(() => _dbPerformerType.GetAsync(A<Expression<Func<PerformerType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<PerformerType>(null));
            A.CallTo(() => _mapper.Map<PerformerType>(dto)).Returns(entity);
            A.CallTo(() => _dbPerformerType.CreateAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<PerformerTypeDTO>(entity)).Returns(dto);

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.CreatePerformerType(dto);

            var created = result.Result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreatePerformerType_Duplicate_ReturnsBadRequest()
        {
            var dto = new PerformerTypeDTO { Name = "Duplicate" };
            var entity = new PerformerType { Name = "Duplicate" };

            A.CallTo(() => _dbPerformerType.GetAsync(A<Expression<Func<PerformerType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.CreatePerformerType(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeletePerformerType_InvalidId_ReturnsBadRequest()
        {
            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.DeletePerformerType(0);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeletePerformerType_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPerformerType.GetAsync(A<Expression<Func<PerformerType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<PerformerType>(null));

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.DeletePerformerType(999);
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeletePerformerType_Valid_ReturnsNoContent()
        {
            var entity = new PerformerType { PerformerTypeId = 1, Name = "DJ" };

            A.CallTo(() => _dbPerformerType.GetAsync(A<Expression<Func<PerformerType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbPerformerType.DeletePerformerTypeAsync(entity)).Returns(Task.CompletedTask);

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.DeletePerformerType(1);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePerformerType_Valid_ReturnsOk()
        {
            var dto = new PerformerTypeDTO { PerformerTypeId = 1, Name = "Updated" };
            var entity = new PerformerType { PerformerTypeId = 1, Name = "Updated" };

            A.CallTo(() => _mapper.Map<PerformerType>(dto)).Returns(entity);
            A.CallTo(() => _dbPerformerType.UpdatePerformerTypeAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.UpdatePerformerType(1, dto);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePartialPerformerType_Valid_ReturnsNoContent()
        {
            var entity = new PerformerType { PerformerTypeId = 1, Name = "OldName" };
            var dto = new PerformerTypeDTO { PerformerTypeId = 1, Name = "OldName" };

            A.CallTo(() => _dbPerformerType.GetAsync(A<Expression<Func<PerformerType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<PerformerTypeDTO>(entity)).Returns(dto);
            A.CallTo(() => _mapper.Map<PerformerType>(dto)).Returns(entity);
            A.CallTo(() => _dbPerformerType.UpdatePerformerTypeAsync(entity)).Returns(Task.FromResult(entity));

            var patchDoc = new JsonPatchDocument<PerformerTypeDTO>();
            patchDoc.Replace(x => x.Name, "NewName");

            var controller = new PerformerTypeController(_dbPerformerType, _mapper, _cacheService);
            var result = await controller.UpdatePartialPerformerType(1, patchDoc);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
