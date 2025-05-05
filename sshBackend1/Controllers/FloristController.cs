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
    public class FloristController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IFloristRepository _dbFlorist;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FloristController(IFloristRepository dbFlorist, IMapper mapper, ICacheService cacheService)
        {
            _dbFlorist = dbFlorist;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetFlorists()
        {
            try
            {
                var floristList = await _cacheService.GetOrAddAsync("floristListCache",
                   async () => await _dbFlorist.GetAllAsync(),
                   TimeSpan.FromMinutes(1));
                //IEnumerable<Florist> floristList = await _dbFlorist.GetAllAsync();
                _response.Result = _mapper.Map<List<FloristDTO>>(floristList);
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

        [HttpGet("{id:int}", Name = "GetFlorist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetFlorist(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var floristEntity = await _dbFlorist.GetAsync(u => u.FloristId == id);
                if (floristEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<FloristDTO>(floristEntity);
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
        public async Task<ActionResult<APIResponse>> CreateFlorist([FromBody] FloristDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid florist data.");
                }

                if (await _dbFlorist.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Florist already exists!");
                    return BadRequest(ModelState);
                }

                Florist floristEntity = _mapper.Map<Florist>(createDTO);
                await _dbFlorist.CreateAsync(floristEntity);

                _response.Result = _mapper.Map<FloristDTO>(floristEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetFlorist", new { id = floristEntity.FloristId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteFlorist")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteFlorist(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var floristEntity = await _dbFlorist.GetAsync(u => u.FloristId == id);
                if (floristEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbFlorist.DeleteFloristAsync(floristEntity);

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
        [HttpPut("{id:int}", Name = "UpdateFlorist")]
        public async Task<ActionResult<APIResponse>> UpdateFlorist(int id, [FromBody] FloristDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.FloristId)
                {
                    return BadRequest();
                }

                Florist model = _mapper.Map<Florist>(updateDTO);

                await _dbFlorist.UpdateFloristAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialFlorist")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialFlorist(int id, JsonPatchDocument<FloristDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();

            }

            var florist = await _dbFlorist.GetAsync(u => u.FloristId == id, tracked: false);

            FloristDTO floristDTO = _mapper.Map<FloristDTO>(florist);

            if (florist == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(floristDTO, ModelState);
            Florist model = _mapper.Map<Florist>(floristDTO);

            await _dbFlorist.UpdateFloristAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
