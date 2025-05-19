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
    public class VenueProviderControllerTest
    {
        private readonly IVenueProviderRepository _dbVenueProvider;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public VenueProviderControllerTest()
        {
            _dbVenueProvider = A.Fake<IVenueProviderRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetVenueProviders_ReturnsOk()
        {
            var entities = new List<VenueProvider>();
            var dtos = new List<VenueProviderDTO>();

            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored,
                A<Func<Task<IEnumerable<VenueProvider>>>>.Ignored,
                A<TimeSpan>.Ignored)).Returns(Task.FromResult<IEnumerable<VenueProvider>>(entities));

            A.CallTo(() => _mapper.Map<List<VenueProviderDTO>>(entities)).Returns(dtos);

            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.GetVenueProviders();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetVenueProvider_InvalidId_ReturnsBadRequest()
        {
            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.GetVenueProvider(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetVenueProvider_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbVenueProvider.GetAsync(A<Expression<Func<VenueProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<VenueProvider>(null));

            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.GetVenueProvider(123);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateVenueProvider_Valid_ReturnsCreated()
        {
            var dto = new VenueProviderDTO { Name = "New Venue" };
            var entity = new VenueProvider { VenueProviderId = 1, Name = "New Venue" };

            A.CallTo(() => _dbVenueProvider.GetAsync(A<Expression<Func<VenueProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<VenueProvider>(null));

            A.CallTo(() => _mapper.Map<VenueProvider>(dto)).Returns(entity);
            A.CallTo(() => _dbVenueProvider.CreateAsync(entity)).Returns(Task.CompletedTask);

            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.CreateVenueProvider(dto);

            var created = result.Result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateVenueProvider_Duplicate_ReturnsBadRequest()
        {
            var dto = new VenueProviderDTO { Name = "Existing" };
            var entity = new VenueProvider { Name = "Existing" };

            A.CallTo(() => _dbVenueProvider.GetAsync(A<Expression<Func<VenueProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.CreateVenueProvider(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteVenueProvider_Valid_ReturnsOk()
        {
            var entity = new VenueProvider { VenueProviderId = 1 };

            A.CallTo(() => _dbVenueProvider.GetAsync(A<Expression<Func<VenueProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbVenueProvider.DeleteVenueProviderAsync(entity)).Returns(Task.CompletedTask);

            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.DeleteVenueProvider(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateVenueProvider_Valid_ReturnsOk()
        {
            var dto = new VenueProviderDTO { VenueProviderId = 1, Name = "Updated" };
            var entity = new VenueProvider { VenueProviderId = 1, Name = "Updated" };

            A.CallTo(() => _mapper.Map<VenueProvider>(dto)).Returns(entity);
            A.CallTo(() => _dbVenueProvider.UpdateVenueProviderAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.UpdateVenueProvider(1, dto);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePartialVenueProvider_Valid_ReturnsNoContent()
        {
            var entity = new VenueProvider { VenueProviderId = 1, Name = "Old" };
            var dto = new VenueProviderDTO { VenueProviderId = 1, Name = "Old" };

            A.CallTo(() => _dbVenueProvider.GetAsync(A<Expression<Func<VenueProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<VenueProviderDTO>(entity)).Returns(dto);
            A.CallTo(() => _mapper.Map<VenueProvider>(dto)).Returns(entity);

            var patchDoc = new JsonPatchDocument<VenueProviderDTO>();
            patchDoc.Replace(x => x.Name, "New");

            var controller = new VenueProviderController(_dbVenueProvider, _mapper, _cacheService);
            var result = await controller.UpdatePartialVenueProvider(1, patchDoc);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
