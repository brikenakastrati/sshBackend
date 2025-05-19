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
    public class PartnerStatusControllerTest
    {
        private readonly IPartnerStatusRepository _dbPartnerStatus;
        private readonly IMapper _mapper;

        public PartnerStatusControllerTest()
        {
            _dbPartnerStatus = A.Fake<IPartnerStatusRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task GetPartnerStatuses_ReturnsOk()
        {
            var entityList = new List<PartnerStatus>
            {
                new PartnerStatus { PartnerStatusId = 1, Name = "Active" },
                new PartnerStatus { PartnerStatusId = 2, Name = "Inactive" }
            };

            var dtoList = new List<PartnerStatusDTO>
            {
                new PartnerStatusDTO { PartnerStatusId = 1, Name = "Active" },
                new PartnerStatusDTO { PartnerStatusId = 2, Name = "Inactive" }
            };

            A.CallTo(() => _dbPartnerStatus.GetAllPartnerStatusesAsync(null))
                .Returns(Task.FromResult<IEnumerable<PartnerStatus>>(entityList));

            A.CallTo(() => _mapper.Map<List<PartnerStatusDTO>>(entityList)).Returns(dtoList);

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.GetPartnerStatuses();
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            var response = okResult!.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().BeEquivalentTo(dtoList);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPartnerStatus_InvalidId_ReturnsBadRequest()
        {
            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.GetPartnerStatus(-1);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            ((APIResponse)badRequest!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetPartnerStatus_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPartnerStatus.GetPartnerStatusAsync(A<Expression<Func<PartnerStatus, bool>>>._))
                .Returns(Task.FromResult<PartnerStatus>(null));

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.GetPartnerStatus(100);

            var notFound = result.Result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            ((APIResponse)notFound!.Value!).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetPartnerStatus_ValidId_ReturnsOk()
        {
            var entity = new PartnerStatus { PartnerStatusId = 1, Name = "Test" };
            var dto = new PartnerStatusDTO { PartnerStatusId = 1, Name = "Test" };

            A.CallTo(() => _dbPartnerStatus.GetPartnerStatusAsync(A<Expression<Func<PartnerStatus, bool>>>._))
                .Returns(Task.FromResult(entity));

            A.CallTo(() => _mapper.Map<PartnerStatusDTO>(entity)).Returns(dto);

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.GetPartnerStatus(1);
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            var response = okResult!.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task CreatePartnerStatus_NullDto_ReturnsBadRequest()
        {
            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.CreatePartnerStatus(null);
            var badRequest = result.Result as BadRequestObjectResult;

            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Invalid partner status data.");
        }

        [Fact]
        public async Task CreatePartnerStatus_DuplicateName_ReturnsBadRequest()
        {
            var dto = new PartnerStatusDTO { Name = "Duplicate" };
            var existing = new PartnerStatus { Name = "Duplicate" };

            A.CallTo(() => _dbPartnerStatus.GetPartnerStatusAsync(A<Expression<Func<PartnerStatus, bool>>>._))
                .Returns(Task.FromResult(existing));

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.CreatePartnerStatus(dto);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
        }

        [Fact]
        public async Task CreatePartnerStatus_ValidDto_ReturnsCreatedAtRoute()
        {
            var dto = new PartnerStatusDTO { Name = "New" };
            var entity = new PartnerStatus { PartnerStatusId = 1, Name = "New" };
            var mappedDto = new PartnerStatusDTO { PartnerStatusId = 1, Name = "New" };

            A.CallTo(() => _dbPartnerStatus.GetPartnerStatusAsync(A<Expression<Func<PartnerStatus, bool>>>._))
                .Returns(Task.FromResult<PartnerStatus>(null));

            A.CallTo(() => _mapper.Map<PartnerStatus>(dto)).Returns(entity);
            A.CallTo(() => _dbPartnerStatus.CreatePartnerStatusAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<PartnerStatusDTO>(entity)).Returns(mappedDto);

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.CreatePartnerStatus(dto);

            var created = result.Result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
            ((APIResponse)created!.Value!).Result.Should().BeEquivalentTo(mappedDto);
        }

        [Fact]
        public async Task DeletePartnerStatus_InvalidId_ReturnsBadRequest()
        {
            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.DeletePartnerStatus(-1);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            ((APIResponse)badRequest!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeletePartnerStatus_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPartnerStatus.GetPartnerStatusAsync(A<Expression<Func<PartnerStatus, bool>>>._))
                .Returns(Task.FromResult<PartnerStatus>(null));

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.DeletePartnerStatus(100);

            var notFound = result.Result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            ((APIResponse)notFound!.Value!).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeletePartnerStatus_ValidId_ReturnsOk()
        {
            var partner = new PartnerStatus { PartnerStatusId = 1, Name = "DeleteMe" };

            A.CallTo(() => _dbPartnerStatus.GetPartnerStatusAsync(A<Expression<Func<PartnerStatus, bool>>>._))
                .Returns(Task.FromResult(partner));

            A.CallTo(() => _dbPartnerStatus.DeletePartnerStatusAsync(partner)).Returns(Task.CompletedTask);

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.DeletePartnerStatus(1);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePartnerStatus_ValidDto_ReturnsOk()
        {
            var dto = new PartnerStatusDTO { PartnerStatusId = 1, Name = "Updated" };
            var model = new PartnerStatus { PartnerStatusId = 1, Name = "Updated" };

            A.CallTo(() => _mapper.Map<PartnerStatus>(dto)).Returns(model);
            A.CallTo(() => _dbPartnerStatus.UpdatePartnerStatusAsync(model)).Returns(Task.FromResult(model));

            var controller = new PartnerStatusController(_dbPartnerStatus, _mapper);
            var result = await controller.UpdatePartnerStatus(1, dto);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
