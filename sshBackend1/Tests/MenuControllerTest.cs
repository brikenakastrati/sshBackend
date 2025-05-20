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
    public class MenuControllerTest
    {
        private readonly IMenuRepository _dbMenu;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        protected APIResponse _apiResponse;

        public MenuControllerTest()
        {
            _dbMenu = A.Fake<IMenuRepository>();
            _apiResponse = A.Fake<APIResponse>();
            _cacheService = A.Fake<ICacheService>();
            _mapper = A.Fake<IMapper>();
        }
        [Fact]

        //GET ALL MenuS UNIT TEST
        public async Task MenuController_GetMenus_ReturnOk()
        {
            // Arrange
            var Menus = A.Fake<ICollection<MenuDTO>>();
            var MenuList = A.Fake<List<MenuDTO>>();
            A.CallTo(() => _mapper.Map<List<MenuDTO>>(Menus)).Returns(MenuList);
            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenus(); // ActionResult<APIResponse>
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull(); // Kontrollon që është kthyer 200 OK
            okResult!.Value.Should().BeOfType<APIResponse>(); // Kontrollon tipin e përmbajtjes
        }
        //GET Menu : 1. Invalid ID, 2. Valid ID, 3. Not found ID

        //1. Invalid ID unit test
        [Fact]
        public async Task GetMenu_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenu(invalidId);
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
        public async Task GetMenu_WithValidId_ReturnsOk()
        {
            // Arrange
            int MenuId = 1;
            var MenuEntity = new Menu { MenuId = MenuId }; // përdor instancë reale, jo Fake
            var MenuDto = new MenuDTO { MenuId = MenuId };       // e njëjta për DTO

            A.CallTo(() => _dbMenu.GetAsync(
                A<Expression<Func<Menu, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult(MenuEntity));

            A.CallTo(() => _mapper.Map<MenuDTO>(MenuEntity)).Returns(MenuDto);

            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenu(MenuId);
            var okResult = actionResult.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.Result.Should().Be(MenuDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //3. Not found ID unit test
        [Fact]
        public async Task GetMenu_NotFound_ReturnsNotFound()
        {
            // Arrange
            int MenuId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMenu.GetAsync(A<Expression<Func<Menu, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<Menu>(null));


            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var actionResult = await controller.GetMenu(MenuId);
            var notFoundResult = actionResult.Result as NotFoundObjectResult;

            // Assert
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var response = notFoundResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }



        //CREATE Menu: 1. Valid Input , 2. Dublicate Menu Name, 3. Null MenuDTO

        //1. Valid Input unit test
        [Fact]
        public async Task CreateMenu_ValidInput_ReturnsCreatedAtRoute()
        {
            // Arrange
            var createDto = new MenuDTO { MenuName = "Test Menu" };
            var createdEntity = new Menu { MenuId = 1, Chef_Name = "Test Menu" };
            var createdDto = new MenuDTO { MenuId = 1, MenuName = "Test Menu" };

            A.CallTo(() => _dbMenu.GetAsync(
                A<Expression<Func<Menu, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<Menu>(null)); // No duplicate

            A.CallTo(() => _mapper.Map<Menu>(createDto)).Returns(createdEntity);
            A.CallTo(() => _dbMenu.CreateAsync(createdEntity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<MenuDTO>(createdEntity)).Returns(createdDto);

            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenu(createDto);
            var createdResult = result.Result as CreatedAtRouteResult;

            // Assert
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

            var response = createdResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().BeEquivalentTo(createdDto);
        }
        //2. Dublicate Menu Name unit test
        [Fact]
        public async Task CreateMenu_DuplicateMenu_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new MenuDTO { MenuName = "Duplicate Menu" };
            var existingMenu = new Menu { MenuId = 1, Chef_Name = "Duplicate Menu" };

            A.CallTo(() => _dbMenu.GetAsync(
                A<Expression<Func<Menu, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(existingMenu)); // Simulate duplicate

            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenu(createDto);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        //3. Null MenuDTO unit test
        [Fact]
        public async Task CreateMenu_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var result = await controller.CreateMenu(null);
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequestResult.Value.Should().Be("Invalid Menu data.");
        }

        //DELETE Menu: 1. Invalid ID, 2. Null Entity, 3. Succesful Delete

        //Invalid ID unit testing
        [Fact]
        public async Task DeleteMenu_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = -1;
            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var actionResult = await controller.DeleteMenu(invalidId);
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
        public async Task DeleteMenu_NullInput_ReturnsBadRequest()
        {

            // Arrange
            int MenuId = 100;
            // e njëjta për DTO
            A.CallTo(() => _dbMenu.GetAsync(A<Expression<Func<Menu, bool>>>._, A<bool>._, A<string>._)).Returns(Task.FromResult<Menu>(null));
            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMenu(MenuId);
            var notFoundResult = result.Result as NotFoundObjectResult;

            // Assert

            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        }

        //3. Succesful delete unit test
        [Fact]
        public async Task DeleteMenu_ValidId_ReturnsOkAndNoContentStatus()
        {
            // Arrange
            int MenuId = 1;
            var fakeMenu = new Menu
            {
                MenuId = MenuId,
                Chef_Name = "Sample Menu"
            };

            A.CallTo(() => _dbMenu.GetAsync(A<Expression<Func<Menu, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(fakeMenu));

            A.CallTo(() => _dbMenu.DeleteMenuAsync(fakeMenu))
                .Returns(Task.CompletedTask);

            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var result = await controller.DeleteMenu(MenuId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var response = okResult.Value as APIResponse;
            response.Should().NotBeNull();
            response!.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Also verify that DeleteMenuAsync was called
            A.CallTo(() => _dbMenu.DeleteMenuAsync(A<Menu>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task UpdateMenu_ValidDto_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            int MenuId = 1;
            var updateDto = new MenuDTO { MenuId = MenuId, MenuName = "Updated Menu" };
            var mappedMenu = new Menu { MenuId = MenuId, Chef_Name = "Updated Menu" };

            A.CallTo(() => _mapper.Map<Menu>(updateDto)).Returns(mappedMenu);
            A.CallTo(() => _dbMenu.UpdateMenuAsync(A<Menu>.Ignored)).Returns(Task.FromResult(mappedMenu));

            var controller = new MenuController(_dbMenu, _mapper, _cacheService);

            // Act
            var result = await controller.UpdateMenu(MenuId, updateDto);
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