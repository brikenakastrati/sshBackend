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
    public class PlaylistItemControllerTest
    {
        private readonly IPlaylistItemRepository _dbPlaylistItem;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public PlaylistItemControllerTest()
        {
            _dbPlaylistItem = A.Fake<IPlaylistItemRepository>();
            _mapper = A.Fake<IMapper>();
            _cacheService = A.Fake<ICacheService>();
        }

        [Fact]
        public async Task GetPlaylistItems_ReturnsOk()
        {
            var list = new List<PlaylistItem>();
            var dtoList = new List<PlaylistItemDTO>();

            A.CallTo(() => _cacheService.GetOrAddAsync(
                A<string>.Ignored,
                A<Func<Task<IEnumerable<PlaylistItem>>>>.Ignored,
                A<TimeSpan>.Ignored)).Returns(Task.FromResult<IEnumerable<PlaylistItem>>(list));

            A.CallTo(() => _mapper.Map<List<PlaylistItemDTO>>(list)).Returns(dtoList);

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.GetPlaylistItems();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPlaylistItem_InvalidId_ReturnsBadRequest()
        {
            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.GetPlaylistItem(-1);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetPlaylistItem_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPlaylistItem.GetAsync(A<Expression<Func<PlaylistItem, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<PlaylistItem>(null));

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.GetPlaylistItem(999);
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreatePlaylistItem_Valid_ReturnsCreated()
        {
            var dto = new PlaylistItemDTO { Name = "New Song" };
            var entity = new PlaylistItem { PlaylistItemId = 1, Name = "New Song" };

            A.CallTo(() => _dbPlaylistItem.GetAsync(A<Expression<Func<PlaylistItem, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<PlaylistItem>(null));
            A.CallTo(() => _mapper.Map<PlaylistItem>(dto)).Returns(entity);
            A.CallTo(() => _dbPlaylistItem.CreateAsync(entity)).Returns(Task.CompletedTask);
            A.CallTo(() => _mapper.Map<PlaylistItemDTO>(entity)).Returns(dto);

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.CreatePlaylistItem(dto);

            var created = result.Result as CreatedAtRouteResult;
            created.Should().NotBeNull();
            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreatePlaylistItem_Duplicate_ReturnsBadRequest()
        {
            var dto = new PlaylistItemDTO { Name = "Duplicate" };
            var entity = new PlaylistItem { Name = "Duplicate" };

            A.CallTo(() => _dbPlaylistItem.GetAsync(A<Expression<Func<PlaylistItem, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.CreatePlaylistItem(dto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeletePlaylistItem_InvalidId_ReturnsBadRequest()
        {
            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.DeletePlaylistItem(0);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeletePlaylistItem_NotFound_ReturnsNotFound()
        {
            A.CallTo(() => _dbPlaylistItem.GetAsync(A<Expression<Func<PlaylistItem, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult<PlaylistItem>(null));

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.DeletePlaylistItem(999);
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeletePlaylistItem_Valid_ReturnsNoContent()
        {
            var entity = new PlaylistItem { PlaylistItemId = 1, Name = "To Delete" };

            A.CallTo(() => _dbPlaylistItem.GetAsync(A<Expression<Func<PlaylistItem, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _dbPlaylistItem.DeletePlaylistItemAsync(entity)).Returns(Task.CompletedTask);

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.DeletePlaylistItem(1);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePlaylistItem_Valid_ReturnsOk()
        {
            var dto = new PlaylistItemDTO { PlaylistItemId = 1, Name = "Updated Name" };
            var entity = new PlaylistItem { PlaylistItemId = 1, Name = "Updated Name" };

            A.CallTo(() => _mapper.Map<PlaylistItem>(dto)).Returns(entity);
            A.CallTo(() => _dbPlaylistItem.UpdatePlaylistItemAsync(entity)).Returns(Task.FromResult(entity));

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.UpdatePlaylistItem(1, dto);
            var ok = result.Result as OkObjectResult;

            ok.Should().NotBeNull();
            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdatePartialPlaylistItem_Valid_ReturnsNoContent()
        {
            var entity = new PlaylistItem { PlaylistItemId = 1, Name = "Old" };
            var dto = new PlaylistItemDTO { PlaylistItemId = 1, Name = "Old" };

            A.CallTo(() => _dbPlaylistItem.GetAsync(A<Expression<Func<PlaylistItem, bool>>>._, A<bool>._, A<string>._))
                .Returns(Task.FromResult(entity));
            A.CallTo(() => _mapper.Map<PlaylistItemDTO>(entity)).Returns(dto);
            A.CallTo(() => _mapper.Map<PlaylistItem>(dto)).Returns(entity);
            A.CallTo(() => _dbPlaylistItem.UpdatePlaylistItemAsync(entity)).Returns(Task.FromResult(entity));

            var patchDoc = new JsonPatchDocument<PlaylistItemDTO>();
            patchDoc.Replace(x => x.Name, "Updated");

            var controller = new PlaylistItemController(_dbPlaylistItem, _mapper, _cacheService);
            var result = await controller.UpdatePartialPlaylistItem(1, patchDoc);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
