// xUnit Test Script for VenueTypeController
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
    public class VenueTypeControllerTests
    {
        private readonly IVenueTypeRepository _venueTypeRepo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public VenueTypeControllerTests()
        {
            _venueTypeRepo = A.Fake<IVenueTypeRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetVenueTypes_ReturnsOkResult()
        {
            var data = A.Fake<List<VenueType>>();
            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored, A<Func<Task<IEnumerable<VenueType>>>>.Ignored, A<TimeSpan>.Ignored))
                .Returns(Task.FromResult<IEnumerable<VenueType>>(data));

            var mapped = A.Fake<List<VenueTypeDTO>>();
            A.CallTo(() => _mapper.Map<List<VenueTypeDTO>>(data)).Returns(mapped);

            var controller = new VenueTypeController(_venueTypeRepo, _mapper, _cacheService);
            var result = await controller.GetVenueTypes();

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetVenueType_InvalidId_ReturnsBadRequest()
        {
            var controller = new VenueTypeController(_venueTypeRepo, _mapper, _cacheService);
            var result = await controller.GetVenueType(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetVenueType_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _venueTypeRepo.GetAsync(A<Expression<Func<VenueType, bool>>>.Ignored, A<bool>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult<VenueType>(null));

            var controller = new VenueTypeController(_venueTypeRepo, _mapper, _cacheService);
            var result = await controller.GetVenueType(100);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateVenueType_Valid_ReturnsCreated()
        {
            var dto = new VenueTypeDTO { Name = "Concert Hall" };
            A.CallTo(() => _venueTypeRepo.GetAsync(A<Expression<Func<VenueType, bool>>>.Ignored, A<bool>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult<VenueType>(null));

            var entity = new VenueType { Name = "Concert Hall" };
            A.CallTo(() => _mapper.Map<VenueType>(dto)).Returns(entity);
            A.CallTo(() => _mapper.Map<VenueTypeDTO>(entity)).Returns(dto);

            var controller = new VenueTypeController(_venueTypeRepo, _mapper, _cacheService);
            var result = await controller.CreateVenueType(dto);

            result.Result.Should().BeOfType<CreatedAtRouteResult>();
        }

        [Fact]
        public async Task DeleteVenueType_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _venueTypeRepo.GetAsync(A<Expression<Func<VenueType, bool>>>.Ignored, A<bool>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult<VenueType>(null));

            var controller = new VenueTypeController(_venueTypeRepo, _mapper, _cacheService);
            var result = await controller.DeleteVenueType(5);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdateVenueType_Valid_ReturnsOk()
        {
            var dto = new VenueTypeDTO { VenueTypeId = 1, Name = "Theater" };
            var entity = new VenueType { VenueTypeId = 1, Name = "Theater" };

            A.CallTo(() => _mapper.Map<VenueType>(dto)).Returns(entity);

            var controller = new VenueTypeController(_venueTypeRepo, _mapper, _cacheService);
            var result = await controller.UpdateVenueType(1, dto);

            result.Result.Should().BeOfType<OkObjectResult>();
        }
    }
}
