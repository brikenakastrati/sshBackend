using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Controllers;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class MusicProviderControllerTest
    {
        private readonly IMusicProviderRepository _dbMusicProvider;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public MusicProviderControllerTest()
        {
            _dbMusicProvider = A.Fake<IMusicProviderRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]

        //GET ALL MusicProviderS UNIT TEST
        public async Task MusicProviderController_GetMusicProviders_ReturnOk()
        {
            // Arrange
            var MusicProviders = A.Fake<ICollection<MusicProviderDTO>>();
            var MusicProviderList = A.Fake<List<MusicProviderDTO>>();
            A.CallTo(() => _mapper.Map<List<MusicProviderDTO>>(MusicProviders)).Returns(MusicProviderList);
            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProviders(); // ActionResult<APIResponse>
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull(); // Kontrollon që është kthyer 200 OK
            okResult!.Value.Should().BeOfType<APIResponse>(); // Kontrollon tipin e përmbajtjes
        }
        //GET MusicProvider : 1. Invalid ID, 2. Valid ID, 3. Not found ID

        //1. Invalid ID unit test
        [Fact]
        public async Task GetMusicProvider_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProvider(invalidId);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var response = badRequestResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //2. Valid ID unit test
        [Fact]
        public async Task GetMusicProvider_WithValidId_ReturnsOk()
        {
            // Arrange
            int MusicProviderId = 1;
            var MusicProviderEntity = new MusicProvider { MusicProviderId = MusicProviderId }; // përdor instancë reale, jo Fake
            var MusicProviderDto = new MusicProviderDTO { MusicProviderId = MusicProviderId };       // e njëjta për DTO

            A.CallTo(() => _dbMusicProvider.GetAsync(
                A<Expression<Func<MusicProvider, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult(MusicProviderEntity));

            A.CallTo(() => _mapper.Map<MusicProviderDTO>(MusicProviderEntity)).Returns(MusicProviderDto);

            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProvider(MusicProviderId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(MusicProviderDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //3. Not found ID unit test
        [Fact]
        public async Task GetMusicProvider_NotFound_ReturnsNotFound()
        {
            // Arrange
            int MusicProviderId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMusicProvider.GetAsync(A<Expression<Func<MusicProvider, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MusicProvider>(null));


            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMusicProvider(MusicProviderId);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }



        //CREATE MusicProvider: 1. Valid Input , 2. Dublicate MusicProvider Name, 3. Null MusicProviderDTO

        //1. Valid Input unit test
        [Fact]
        public async Task CreateMusicProvider_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new MusicProviderDTO { Name = "Test MusicProvider" };
            var createdEntity = new MusicProvider { MusicProviderId = 1, Name = "Test MusicProvider" };
            var createdDto = new MusicProviderDTO { MusicProviderId = 1, Name = "Test MusicProvider" };

            A.CallTo(() => _dbMusicProvider.GetAsync(
                A<Expression<Func<MusicProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<MusicProvider>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<MusicProvider>(createDto)).Returns(createdEntity);
            A.CallTo(() => _dbMusicProvider.CreateAsync(createdEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<MusicProviderDTO>(createdEntity)).Returns(createdDto);

            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMusicProvider(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(createdDto);
        }
        //2. Dublicate MusicProvider Name unit test
        [Fact]
        public async Task CreateMusicProvider_DuplicateMusicProvider_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new MusicProviderDTO { Name = "Duplicate MusicProvider" };
            var existingMusicProvider = new MusicProvider { MusicProviderId = 1, Name = "Duplicate MusicProvider" };

            A.CallTo(() => _dbMusicProvider.GetAsync(
                A<Expression<Func<MusicProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingMusicProvider)); // Simulate duplicate

            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMusicProvider(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        //3. Null MusicProviderDTO unit test
        [Fact]
        public async Task CreateMusicProvider_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMusicProvider(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid Music Provider data.");
        }

        //DELETE MusicProvider: 1. Invalid ID, 2. Null Entity, 3. Succesful Delete

        //Invalid ID unit testing
        [Fact]
        public async Task DeleteMusicProvider_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var actionResult = await controller.DeleteMusicProvider(invalidId);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var response = badRequestResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //2. Null Entity unit test
        [Fact]
        public async Task DeleteMusicProvider_NullInput_ReturnsBadRequest()
        {

            // Arrange
            int MusicProviderId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMusicProvider.GetAsync(A<Expression<Func<MusicProvider, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MusicProvider>(null));
            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMusicProvider(MusicProviderId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert

            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }

        //3. Succesful delete unit test
        [Fact]
        public async Task DeleteMusicProvider_ValidId_ReturnsOkAndNoContentStatus()
        {
            // Arrange
            int MusicProviderId = 1;
            var fakeMusicProvider = new MusicProvider
            {
                MusicProviderId = MusicProviderId,
                Name = "Sample MusicProvider"
            };

            A.CallTo(() => _dbMusicProvider.GetAsync(A<Expression<Func<MusicProvider, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(fakeMusicProvider));

            A.CallTo(() => _dbMusicProvider.DeleteMusicProviderAsync(fakeMusicProvider))
                .Returns(Task.CompletedTask);

            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMusicProvider(MusicProviderId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Also verify that DeleteMusicProviderAsync was called
            A.CallTo(() => _dbMusicProvider.DeleteMusicProviderAsync(A<MusicProvider>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task UpdateMusicProvider_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int MusicProviderId = 1;
            var updateDto = new MusicProviderDTO { MusicProviderId = MusicProviderId, Name = "Updated MusicProvider" };
            var mappedMusicProvider = new MusicProvider { MusicProviderId = MusicProviderId, Name = "Updated MusicProvider" };

            A.CallTo(() => _mapper.Map<MusicProvider>(updateDto)).Returns(mappedMusicProvider);
            A.CallTo(() => _dbMusicProvider.UpdateMusicProviderAsync(A<MusicProvider>.Ignored)).Returns(Task.FromResult(mappedMusicProvider));

            var controller = new MusicProviderController(_dbMusicProvider, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateMusicProvider(MusicProviderId, updateDto);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
        }

    };

}