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
    public class VenueControllerTest
    {
        private readonly IVenueRepository _dbVenue;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public VenueControllerTest()
        {
            _dbVenue = A.Fake<IVenueRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetAllVenues_ReturnsOk()
        {
            var venues = new List<Venue>();
            var venueDtos = new List<VenueDTO>();

            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored,
                A<Func<Task<IEnumerable<Venue>>>>.Ignored,
                A<TimeSpan>.Ignored)).Returns(Task.FromResult<IEnumerable<Venue>>(venues));

            A.CallTo(() => _mapper.Map<List<VenueDTO>>(venues)).Returns(venueDtos);

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.GetAllVenues();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetVenue_InvalidId_ReturnsBadRequest()
        {
            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.GetVenue(-1);
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetVenue_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbVenue.GetAsync(A<Expression<Func<Venue, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Venue>(null));

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.GetVenue(999);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateVenue_Valid_ReturnsCreated()
        {
            var dto = new VenueDTO { Name = "TestVenue" };
            var entity = new Venue { VenueId = 1, Name = "TestVenue" };

            A.CallTo(() => _dbVenue.GetAsync(A<Expression<Func<Venue, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Venue>(null));
            A.CallTo(() => _mapper.Map<Venue>(dto)).Returns(entity);
            A.CallTo(() => _dbVenue.CreateAsync(entity)).Returns(Task.CompletedTask);

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.CreateVenue(dto);

            var created = result.Result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateVenue_Duplicate_ReturnsBadRequest()
        {
            var dto = new VenueDTO { Name = "ExistingVenue" };
            var entity = new Venue { Name = "ExistingVenue" };

            A.CallTo(() => _dbVenue.GetAsync(A<Expression<Func<Venue, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.CreateVenue(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateVenue_Valid_ReturnsOk()
        {
            var dto = new VenueDTO { VenueId = 1, Name = "UpdatedVenue" };
            var entity = new Venue { VenueId = 1, Name = "UpdatedVenue" };

            A.CallTo(() => _mapper.Map<Venue>(dto)).Returns(entity);
            A.CallTo(() => _dbVenue.UpdateVenueAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.UpdateVenue(1, dto);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteVenue_Valid_ReturnsNoContent()
        {
            var entity = new Venue { VenueId = 1 };

            A.CallTo(() => _dbVenue.GetAsync(A<Expression<Func<Venue, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbVenue.DeleteVenueAsync(entity)).Returns(Task.CompletedTask);

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.DeleteVenue(1);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteVenue_InvalidId_ReturnsBadRequest()
        {
            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.DeleteVenue(0);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteVenue_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbVenue.GetAsync(A<Expression<Func<Venue, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Venue>(null));

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.DeleteVenue(123);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdatePartialVenue_Valid_ReturnsNoContent()
        {
            var entity = new Venue { VenueId = 1, Name = "OldName" };
            var dto = new VenueDTO { VenueId = 1, Name = "OldName" };

            A.CallTo(() => _dbVenue.GetAsync(A<Expression<Func<Venue, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<VenueDTO>(entity)).Returns(dto);
            A.CallTo(() => _mapper.Map<Venue>(dto)).Returns(entity);
            A.CallTo(() => _dbVenue.UpdateVenueAsync(entity)).Returns(Task.FromResult(entity));

            var patchDoc = new JsonPatchDocument<VenueDTO>();
            patchDoc.Replace(x => x.Name, "NewName");

            var controller = new VenueController(_dbVenue, _mapper, _cacheService);
            var result = await controller.UpdatePartialVenue(1, patchDoc);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
