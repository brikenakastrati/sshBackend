using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services.IServices;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMenuRepository _dbMenu;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;


        public MenuController(IMenuRepository dbMenu, IMapper mapper, ICacheService cacheService)
        {
            _dbMenu = dbMenu;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMenus()
        {
            try
            {
                var menuList = await _cacheService.GetOrAddAsync("menuListCache",
                    async () => await _dbMenu.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<Menu> menuList = await _dbMenu.GetAllAsync();
                _response.Result = _mapper.Map<List<MenuDTO>>(menuList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{id:int}", Name = "GetMenu")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMenu(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var menuEntity = await _dbMenu.GetAsync(u => u.MenuId == id);
                if (menuEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<MenuDTO>(menuEntity);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateMenu([FromBody] MenuDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Menu data.");
                }

                if (await _dbMenu.GetAsync(u => u.MenuName.ToLower() == createDTO.MenuName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Menu already exists!");
                    return BadRequest(ModelState);
                }

                Menu menuEntity = _mapper.Map<Menu>(createDTO);
                await _dbMenu.CreateAsync(menuEntity);

                _response.Result = _mapper.Map<MenuDTO>(menuEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetMenu", new { id = menuEntity.MenuId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteMenu")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteMenu(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var menuEntity = await _dbMenu.GetAsync(u => u.MenuId == id);
                if (menuEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbMenu.DeleteMenuAsync(menuEntity);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateMenu")]
        public async Task<ActionResult<APIResponse>> UpdateMenu(int id, [FromBody] MenuDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.MenuId)
                {
                    return BadRequest();
                }

                Menu model = _mapper.Map<Menu>(updateDTO);

                await _dbMenu.UpdateMenuAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialMenu")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialMenu(int id, JsonPatchDocument<MenuDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var menuEntity = await _dbMenu.GetAsync(u => u.MenuId == id, tracked: false);

            if (menuEntity == null)
            {
                return BadRequest();
            }

            MenuDTO menuDTO = _mapper.Map<MenuDTO>(menuEntity);
            patchDTO.ApplyTo(menuDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Menu model = _mapper.Map<Menu>(menuDTO);
            await _dbMenu.UpdateMenuAsync(model);

            return NoContent();
        }
    }
}
