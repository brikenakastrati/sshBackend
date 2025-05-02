using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services.IServices;
using System.Net;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuTypeController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMenuTypeRepository _dbMenuType;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public MenuTypeController(IMenuTypeRepository dbMenuType, IMapper mapper, ICacheService cacheService)
        {
            _dbMenuType = dbMenuType;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMenuTypes()
        {
            try
            {
                var menuTypeList = await _cacheService.GetOrAddAsync("menuTypeListCache",
                  async () => await _dbMenuType.GetAllAsync(),
                  TimeSpan.FromMinutes(1));
                //IEnumerable<MenuType> menuTypeList = await _dbMenuType.GetAllAsync();
                _response.Result = _mapper.Map<List<MenuTypeDTO>>(menuTypeList);
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

        [HttpGet("{id:int}", Name = "GetMenuType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMenuType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var menuTypeEntity = await _dbMenuType.GetAsync(u => u.MenuTypeId == id);
                if (menuTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<MenuTypeDTO>(menuTypeEntity);
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
        public async Task<ActionResult<APIResponse>> CreateMenuType([FromBody] MenuTypeDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Menu Type data.");
                }

                if (await _dbMenuType.GetAsync(u => u.TypeName.ToLower() == createDTO.TypeName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Menu Type already exists!");
                    return BadRequest(ModelState);
                }

                MenuType menuTypeEntity = _mapper.Map<MenuType>(createDTO);
                await _dbMenuType.CreateAsync(menuTypeEntity);

                _response.Result = _mapper.Map<MenuTypeDTO>(menuTypeEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetMenuType", new { id = menuTypeEntity.MenuTypeId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteMenuType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteMenuType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var menuTypeEntity = await _dbMenuType.GetAsync(u => u.MenuTypeId == id);
                if (menuTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbMenuType.DeleteMenuTypeAsync(menuTypeEntity);

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
        [HttpPut("{id:int}", Name = "UpdateMenuType")]
        public async Task<ActionResult<APIResponse>> UpdateMenuType(int id, [FromBody] MenuTypeDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.MenuTypeId)
                {
                    return BadRequest();
                }

                MenuType model = _mapper.Map<MenuType>(updateDTO);

                await _dbMenuType.UpdateMenuTypeAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialMenuType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialMenuType(int id, JsonPatchDocument<MenuTypeDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var menuTypeEntity = await _dbMenuType.GetAsync(u => u.MenuTypeId == id, tracked: false);

            if (menuTypeEntity == null)
            {
                return BadRequest();
            }

            MenuTypeDTO menuTypeDTO = _mapper.Map<MenuTypeDTO>(menuTypeEntity);
            patchDTO.ApplyTo(menuTypeDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MenuType model = _mapper.Map<MenuType>(menuTypeDTO);
            await _dbMenuType.UpdateMenuTypeAsync(model);

            return NoContent();
        }


    }
}
