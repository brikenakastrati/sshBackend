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
    public class PastryTypeControllerTest
    {
        private readonly IPastryTypeRepository _dbPastryType;
        private readonly IMapper _mapper;

        public PastryTypeControllerTest()
        {
            _dbPastryType = A.Fake<IPastryTypeRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task GetPastryTypes_ReturnsOk()
        {
            var pastryTypes = new List<PastryType>();
            var dtoList = new List<PastryTypeDTO>();

            A.CallTo(() => _dbPastryType.GetAllPastryTypesAsync(null))
                .Returns(Task.FromResult<IEnumerable<PastryType>>(pastryTypes));

            A.CallTo(() => _mapper.Map<List<PastryTypeDTO>>(pastryTypes)).Returns(dtoList);

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.GetPastryTypes();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPastryType_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.GetPastryType(-1);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetPastryType_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPastryType.GetPastryTypeAsync(A<Expression<Func<PastryType, bool>>>._))
                .Returns(Task.FromResult<PastryType>(null));

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.GetPastryType(100);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreatePastryType_Valid_ReturnsCreated()
        {
            var dto = new PastryTypeDTO { TypeName = "Cream" };
            var entity = new PastryType { PastryTypeId = 1, TypeName = "Cream" };

            A.CallTo(() => _dbPastryType.GetPastryTypeAsync(A<Expression<Func<PastryType, bool>>>._))
                .Returns(Task.FromResult<PastryType>(null));

            A.CallTo(() => _mapper.Map<PastryType>(dto)).Returns(entity);
            A.CallTo(() => _dbPastryType.CreatePastryTypeAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<PastryTypeDTO>(entity)).Returns(dto);

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.CreatePastryType(dto);

            var createdResult = result.Result as CreatedAtRouteResult;
            createdResult.Should().NotBeNull();
            ((APIResponse)createdResult!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreatePastryType_Duplicate_ReturnsBadRequest()
        {
            var dto = new PastryTypeDTO { TypeName = "Duplicate" };
            var entity = new PastryType { TypeName = "Duplicate" };

            A.CallTo(() => _dbPastryType.GetPastryTypeAsync(A<Expression<Func<PastryType, bool>>>._))
                .Returns(Task.FromResult(entity));

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.CreatePastryType(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeletePastryType_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.DeletePastryType(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeletePastryType_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPastryType.GetPastryTypeAsync(A<Expression<Func<PastryType, bool>>>._))
                .Returns(Task.FromResult<PastryType>(null));

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.DeletePastryType(123);
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeletePastryType_Success_ReturnsNoContent()
        {
            var pastryType = new PastryType { PastryTypeId = 1 };

            A.CallTo(() => _dbPastryType.GetPastryTypeAsync(A<Expression<Func<PastryType, bool>>>._))
                .Returns(Task.FromResult(pastryType));

            A.CallTo(() => _dbPastryType.DeletePastryTypeAsync(pastryType)).Returns(Task.CompletedTask);

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.DeletePastryType(1);
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePastryType_Valid_ReturnsOk()
        {
            var dto = new PastryTypeDTO { PastryTypeId = 1, TypeName = "Updated" };
            var entity = new PastryType { PastryTypeId = 1, TypeName = "Updated" };

            A.CallTo(() => _mapper.Map<PastryType>(dto)).Returns(entity);
            A.CallTo(() => _dbPastryType.UpdatePastryTypeAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.UpdatePastryType(1, dto);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task PatchPastryType_Valid_ReturnsNoContent()
        {
            var original = new PastryType { PastryTypeId = 1, TypeName = "Original" };
            var dto = new PastryTypeDTO { PastryTypeId = 1, TypeName = "Original" };

            A.CallTo(() => _dbPastryType.GetPastryTypeAsync(A<Expression<Func<PastryType, bool>>>._))
                .Returns(Task.FromResult(original));

            A.CallTo(() => _mapper.Map<PastryTypeDTO>(original)).Returns(dto);
            A.CallTo(() => _mapper.Map<PastryType>(dto)).Returns(original);
            A.CallTo(() => _dbPastryType.UpdatePastryTypeAsync(original)).Returns(Task.FromResult(original));

            var patchDoc = new JsonPatchDocument<PastryTypeDTO>();
            patchDoc.Replace(x => x.TypeName, "Updated");

            var controller = new PastryTypeController(_dbPastryType, _mapper);
            var result = await controller.UpdatePartialPastryType(1, patchDoc);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
