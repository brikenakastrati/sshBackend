//using AutoMapper;
//using FakeItEasy;
//using FluentAssertions;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using sshBackend1.Controllers;
//using sshBackend1.Models;
//using sshBackend1.Models.DTOs;
//using sshBackend1.Repository.IRepository;
//using sshBackend1.Services.IServices;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;

//namespace UnitTests.ControllerTests
//{
//    public class TableControllerTest
//    {
//        private readonly ITableRepository _dbTable;
//        private readonly IMapper _mapper;
//        private readonly ICacheService _cacheService;

//        public TableControllerTest()
//        {
//            _dbTable = A.Fake<ITableRepository>();
//            _mapper = A.Fake<IMapper>();
//            _cacheService = A.Fake<ICacheService>();
//        }

//        [Fact]
//        public async Task GetTables_ReturnsOk()
//        {
//            var entities = new List<Table>();
//            var dtos = new List<TableDTO>();

//            A.CallTo(() => _cacheService.GetOrAddAsync(
//                A<string>._,
//                A<Func<Task<IEnumerable<Table>>>>._,
//                A<TimeSpan>._)).Returns(Task.FromResult<IEnumerable<Table>>(entities));

//            A.CallTo(() => _mapper.Map<List<TableDTO>>(entities)).Returns(dtos);

//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.GetTables();

//            var okResult = result.Result as OkObjectResult;
//            okResult.Should().NotBeNull();
//            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        [Fact]
//        public async Task GetTable_InvalidId_ReturnsBadRequest()
//        {
//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.GetTable(-1);
//            result.Result.Should().BeOfType<BadRequestObjectResult>();
//        }

//        [Fact]
//        public async Task GetTable_NotFound_ReturnsNotFound()
//        {
//            A.CallTo(() => _dbTable.GetAsync(A<Expression<Func<Table, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult<Table>(null));

//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.GetTable(999);
//            result.Result.Should().BeOfType<NotFoundObjectResult>();
//        }

//        [Fact]
//        public async Task CreateTable_Valid_ReturnsCreated()
//        {
//            var dto = new TableDTO { TableId = 1 };
//            var entity = new Table { TableId = 1 };

//            A.CallTo(() => _mapper.Map<Table>(dto)).Returns(entity);
//            A.CallTo(() => _dbTable.CreateAsync(entity)).Returns(Task.CompletedTask);
//            A.CallTo(() => _mapper.Map<TableDTO>(entity)).Returns(dto);

//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.CreateTable(dto);

//            var created = result.Result as CreatedAtRouteResult;
//            created.Should().NotBeNull();
//            ((APIResponse)created!.Value!).StatusCode.Should().Be(HttpStatusCode.Created);
//        }

//        [Fact]
//        public async Task DeleteTable_InvalidId_ReturnsBadRequest()
//        {
//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.DeleteTable(0);
//            result.Result.Should().BeOfType<BadRequestObjectResult>();
//        }

//        [Fact]
//        public async Task DeleteTable_NotFound_ReturnsNotFound()
//        {
//            A.CallTo(() => _dbTable.GetAsync(A<Expression<Func<Table, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult<Table>(null));

//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.DeleteTable(123);
//            result.Result.Should().BeOfType<NotFoundObjectResult>();
//        }

//        [Fact]
//        public async Task DeleteTable_Valid_ReturnsNoContent()
//        {
//            var entity = new Table { TableId = 1 };

//            A.CallTo(() => _dbTable.GetAsync(A<Expression<Func<Table, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult(entity));
//            A.CallTo(() => _dbTable.DeleteTableAsync(entity)).Returns(Task.CompletedTask);

//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.DeleteTable(1);
//            var okResult = result.Result as OkObjectResult;

//            okResult.Should().NotBeNull();
//            ((APIResponse)okResult!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }

//        [Fact]
//        public async Task UpdateTable_Valid_ReturnsOk()
//        {
//            var dto = new TableDTO { TableId = 1 };
//            var entity = new Table { TableId = 1 };

//            A.CallTo(() => _mapper.Map<Table>(dto)).Returns(entity);
//            A.CallTo(() => _dbTable.UpdateTableAsync(entity)).Returns(Task.FromResult(entity));

//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.UpdateTable(1, dto);
//            var ok = result.Result as OkObjectResult;

//            ok.Should().NotBeNull();
//            ((APIResponse)ok!.Value!).StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }

//        [Fact]
//        public async Task UpdatePartialTable_Valid_ReturnsNoContent()
//        {
//            var entity = new Table { TableId = 1 };
//            var dto = new TableDTO { TableId = 1 };

//            A.CallTo(() => _dbTable.GetAsync(A<Expression<Func<Table, bool>>>._, A<bool>._, A<string>._))
//                .Returns(Task.FromResult(entity));
//            A.CallTo(() => _mapper.Map<TableDTO>(entity)).Returns(dto);
//            A.CallTo(() => _mapper.Map<Table>(dto)).Returns(entity);
//            A.CallTo(() => _dbTable.UpdateTableAsync(entity)).Returns(Task.FromResult(entity));

//            var patchDoc = new JsonPatchDocument<TableDTO>();
//            patchDoc.Replace(x => x.TableId, 1); // Replace any property, adjust as needed

//            var controller = new TableController(_dbTable, _mapper, _cacheService);
//            var result = await controller.UpdatePartialTable(1, patchDoc);

//            result.Should().BeOfType<NoContentResult>();
//        }
//    }
//}
