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
    public class PastryControllerTest
    {
        private readonly IPastryRepository _dbPastry;
        private readonly IMapper _mapper;

        public PastryControllerTest()
        {
            _dbPastry = A.Fake<IPastryRepository>();
            _mapper = A.Fake<IMapper>();
        }

        //[Fact]
        //public async Task GetPastries_ReturnsOk()
        //{
        //    var pastries = new List<Pastry>
        //    {
        //        new Pastry { PastryId = 1, PastryName = "Croissant" },
        //        new Pastry { PastryId = 2, PastryName = "Danish" }
        //    };

        //    var pastryDTOs = new List<PastryDTO>
        //    {
        //        new PastryDTO { PastryId = 1, PastryName = "Croissant" },
        //        new PastryDTO { PastryId = 2, PastryName = "Danish" }
        //    };

        //    A.CallTo(() => _dbPastry.GetAllPastriesAsync(null)).Returns(Task.FromResult((IEnumerable<Pastry>)pastries));
        //    A.CallTo(() => _mapper.Map<List<PastryDTO>>(pastries)).Returns(pastryDTOs);

        //    var controller = new PastryController(_dbPastry, _mapper);
        //    var result = await controller.GetPastries();
        //    var ok = result.Result as OkObjectResult;

        //    ok.Should().NotBeNull();
        //    var response = ok!.Value as APIResponse;
        //    response!.Result.Should().BeEquivalentTo(pastryDTOs);
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}

        [Fact]
        public async Task GetPastry_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryController(_dbPastry, _mapper);
            var result = await controller.GetPastry(-1);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            ((APIResponse)badRequest!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //[Fact]
        //public async Task GetPastry_NotFound_ReturnsNotFound()
        //{
        //    A.CallTo(() => _dbPastry.GetPastryAsync(A<Expression<Func<Pastry, bool>>>._))
        //        .Returns(Task.FromResult<Pastry>(null));

        //    var controller = new PastryController(_dbPastry, _mapper);
        //    var result = await controller.GetPastry(999);

        //    var notFound = result.Result as NotFoundObjectResult;
        //    notFound.Should().NotBeNull();
        //    ((APIResponse)notFound!.Value!).StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async Task GetPastry_ValidId_ReturnsOk()
        //{
        //    var pastry = new Pastry { PastryId = 1, PastryName = "Test" };
        //    var dto = new PastryDTO { PastryId = 1, PastryName = "Test" };

        //    A.CallTo(() => _dbPastry.GetPastryAsync(A<Expression<Func<Pastry, bool>>>._)).Returns(Task.FromResult(pastry));
        //    A.CallTo(() => _mapper.Map<PastryDTO>(pastry)).Returns(dto);

        //    var controller = new PastryController(_dbPastry, _mapper);
        //    var result = await controller.GetPastry(1);

        //    var ok = result.Result as OkObjectResult;
        //    ok.Should().NotBeNull();
        //    ((APIResponse)ok!.Value!).Result.Should().BeEquivalentTo(dto);
        //}

        [Fact]
        public async Task CreatePastry_NullDto_ReturnsBadRequest()
        {
            var controller = new PastryController(_dbPastry, _mapper);
            var result = await controller.CreatePastry(null);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().Be("Invalid pastry data.");
        }

        [Fact]
        public async Task CreatePastry_DuplicateName_ReturnsBadRequest()
        {
            var dto = new PastryDTO { PastryName = "Duplicate" };
            var existing = new Pastry { PastryName = "Duplicate" };

            A.CallTo(() => _dbPastry.GetPastryAsync(A<Expression<Func<Pastry, bool>>>._))
                .Returns(Task.FromResult(existing));

            var controller = new PastryController(_dbPastry, _mapper);
            var result = await controller.CreatePastry(dto);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
        }

        //[Fact]
        //public async Task CreatePastry_ValidDto_ReturnsCreatedAtRoute()
        //{
        //    var dto = new PastryDTO { PastryName = "New" };
        //    var entity = new Pastry { PastryId = 1, PastryName = "New" };

        //    A.CallTo(() => _dbPastry.GetPastryAsync(A<Expression<Func<Pastry, bool>>>._)).Returns(Task.FromResult<Pastry>(null));
        //    A.CallTo(() => _mapper.Map<Pastry>(dto)).Returns(entity);
        //    A.CallTo(() => _dbPastry.CreatePastryAsync(entity)).Returns(Task.CompletedTask);
        //    A.CallTo(() => _mapper.Map<PastryDTO>(entity)).Returns(new PastryDTO { PastryId = 1, PastryName = "New" });

        //    var controller = new PastryController(_dbPastry, _mapper);
        //    var result = await controller.CreatePastry(dto);

        //    var created = result.Result as CreatedAtRouteResult;
        //    created.Should().NotBeNull();
        //    ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        //}

        [Fact]
        public async Task DeletePastry_InvalidId_ReturnsBadRequest()
        {
            var controller = new PastryController(_dbPastry, _mapper);
            var result = await controller.DeletePastry(-1);

            var badRequest = result.Result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            ((APIResponse)badRequest!.Value!).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //[Fact]
        //public async Task DeletePastry_NotFound_ReturnsNotFound()
        //{
        //    A.CallTo(() => _dbPastry.GetPastryAsync(A<Expression<Func<Pastry, bool>>>._)).Returns(Task.FromResult<Pastry>(null));

        //    var controller = new PastryController(_dbPastry, _mapper);
        //    var result = await controller.DeletePastry(100);

        //    var notFound = result.Result as NotFoundObjectResult;
        //    notFound.Should().NotBeNull();
        //    ((APIResponse)notFound!.Value!).StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}

        [Fact]
        public async Task DeletePastry_ValidId_ReturnsOk()
        {
            var pastry = new Pastry { PastryId = 1, PastryName = "DeleteMe" };

            A.CallTo(() => _dbPastry.GetPastryAsync(A<Expression<Func<Pastry, bool>>>._)).Returns(Task.FromResult(pastry));
            A.CallTo(() => _dbPastry.DeletePastryAsync(pastry)).Returns(Task.CompletedTask);

            var controller = new PastryController(_dbPastry, _mapper);
            var result = await controller.DeletePastry(1);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePastry_ValidDto_ReturnsOk()
        {
            var dto = new PastryDTO { PastryId = 1, PastryName = "Updated" };
            var model = new Pastry { PastryId = 1, PastryName = "Updated" };

            A.CallTo(() => _mapper.Map<Pastry>(dto)).Returns(model);
            A.CallTo(() => _dbPastry.UpdatePastryAsync(model)).Returns(Task.FromResult(model));

            var controller = new PastryController(_dbPastry, _mapper);
            var result = await controller.UpdatePastry(1, dto);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
