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
    public class MenuTypeControllerTest
    {
        private readonly IMenuTypeRepository _dbMenuType;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public MenuTypeControllerTest()
        {
            _dbMenuType = A.Fake<IMenuTypeRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]

        //GET ALL MenuTypeS UNIT TEST
        public async Task MenuTypeController_GetMenuTypes_ReturnOk()
        {
            // Arrange
            var MenuTypes = A.Fake<ICollection<MenuTypeDTO>>();
            var MenuTypeList = A.Fake<List<MenuTypeDTO>>();
            A.CallTo(() => _mapper.Map<List<MenuTypeDTO>>(MenuTypes)).Returns(MenuTypeList);
            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuTypes(); // ActionResult<APIResponse>
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull(); // Kontrollon që është kthyer 200 OK
            okResult!.Value.Should().BeOfType<APIResponse>(); // Kontrollon tipin e përmbajtjes
        }
        //GET MenuType : 1. Invalid ID, 2. Valid ID, 3. Not found ID

        //1. Invalid ID unit test
        [Fact]
        public async Task GetMenuType_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuType(invalidId);
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
        public async Task GetMenuType_WithValidId_ReturnsOk()
        {
            // Arrange
            int MenuTypeId = 1;
            var MenuTypeEntity = new MenuType { MenuTypeId = MenuTypeId }; // përdor instancë reale, jo Fake
            var MenuTypeDto = new MenuTypeDTO { MenuTypeId = MenuTypeId };       // e njëjta për DTO

            A.CallTo(() => _dbMenuType.GetAsync(
                A<Expression<Func<MenuType, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult(MenuTypeEntity));

            A.CallTo(() => _mapper.Map<MenuTypeDTO>(MenuTypeEntity)).Returns(MenuTypeDto);

            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuType(MenuTypeId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(MenuTypeDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //3. Not found ID unit test
        [Fact]
        public async Task GetMenuType_NotFound_ReturnsNotFound()
        {
            // Arrange
            int MenuTypeId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMenuType.GetAsync(A<Expression<Func<MenuType, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MenuType>(null));


            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenuType(MenuTypeId);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }



        //CREATE MenuType: 1. Valid Input , 2. Dublicate MenuType Name, 3. Null MenuTypeDTO

        //1. Valid Input unit test
        [Fact]
        public async Task CreateMenuType_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new MenuTypeDTO { TypeName = "Test MenuType" };
            var createdEntity = new MenuType { MenuTypeId = 1, TypeName = "Test MenuType" };
            var createdDto = new MenuTypeDTO { MenuTypeId = 1, TypeName = "Test MenuType" };

            A.CallTo(() => _dbMenuType.GetAsync(
                A<Expression<Func<MenuType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<MenuType>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<MenuType>(createDto)).Returns(createdEntity);
            A.CallTo(() => _dbMenuType.CreateAsync(createdEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<MenuTypeDTO>(createdEntity)).Returns(createdDto);

            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenuType(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(createdDto);
        }
        //2. Dublicate MenuType Name unit test
        [Fact]
        public async Task CreateMenuType_DuplicateMenuType_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new MenuTypeDTO { TypeName = "Duplicate MenuType" };
            var existingMenuType = new MenuType { MenuTypeId = 1, TypeName = "Duplicate MenuType" };

            A.CallTo(() => _dbMenuType.GetAsync(
                A<Expression<Func<MenuType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingMenuType)); // Simulate duplicate

            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenuType(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        //3. Null MenuTypeDTO unit test
        [Fact]
        public async Task CreateMenuType_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenuType(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid Menu Type data.");
        }

        //DELETE MenuType: 1. Invalid ID, 2. Null Entity, 3. Succesful Delete

        //Invalid ID unit testing
        [Fact]
        public async Task DeleteMenuType_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var actionResult = await controller.DeleteMenuType(invalidId);
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
        public async Task DeleteMenuType_NullInput_ReturnsBadRequest()
        {

            // Arrange
            int MenuTypeId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMenuType.GetAsync(A<Expression<Func<MenuType, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<MenuType>(null));
            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMenuType(MenuTypeId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert

            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }

        //3. Succesful delete unit test
        [Fact]
        public async Task DeleteMenuType_ValidId_ReturnsOkAndNoContentStatus()
        {
            // Arrange
            int MenuTypeId = 1;
            var fakeMenuType = new MenuType
            {
                MenuTypeId = MenuTypeId,
                TypeName = "Sample MenuType"
            };

            A.CallTo(() => _dbMenuType.GetAsync(A<Expression<Func<MenuType, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(fakeMenuType));

            A.CallTo(() => _dbMenuType.DeleteMenuTypeAsync(fakeMenuType))
                .Returns(Task.CompletedTask);

            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMenuType(MenuTypeId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Also verify that DeleteMenuTypeAsync was called
            A.CallTo(() => _dbMenuType.DeleteMenuTypeAsync(A<MenuType>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task UpdateMenuType_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int MenuTypeId = 1;
            var updateDto = new MenuTypeDTO { MenuTypeId = MenuTypeId, TypeName = "Updated MenuType" };
            var mappedMenuType = new MenuType { MenuTypeId = MenuTypeId, TypeName = "Updated MenuType" };

            A.CallTo(() => _mapper.Map<MenuType>(updateDto)).Returns(mappedMenuType);
            A.CallTo(() => _dbMenuType.UpdateMenuTypeAsync(A<MenuType>.Ignored)).Returns(Task.FromResult(mappedMenuType));

            var controller = new MenuTypeController(_dbMenuType, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateMenuType(MenuTypeId, updateDto);
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