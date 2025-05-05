using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services.IServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformerTypeController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPerformerTypeRepository _dbPerformerType;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public PerformerTypeController(IPerformerTypeRepository dbPerformerType, IMapper mapper, ICacheService cacheService)
        {
            _dbPerformerType = dbPerformerType;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPerformerTypes()
        {
            try
            {
                var performerTypeList = await _cacheService.GetOrAddAsync("performerTypeListCache",
                    async () => await _dbPerformerType.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<PerformerType> performerTypeList = await _dbPerformerType.GetAllAsync();
                _response.Result = _mapper.Map<List<PerformerTypeDTO>>(performerTypeList);
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

        [HttpGet("{id:int}", Name = "GetPerformerType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPerformerType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var performerTypeEntity = await _dbPerformerType.GetAsync(u => u.PerformerTypeId == id);
                if (performerTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PerformerTypeDTO>(performerTypeEntity);
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
        public async Task<ActionResult<APIResponse>> CreatePerformerType([FromBody] PerformerTypeDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Performer Type data.");
                }

                if (await _dbPerformerType.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Performer Type already exists!");
                    return BadRequest(ModelState);
                }

                PerformerType performerTypeEntity = _mapper.Map<PerformerType>(createDTO);
                await _dbPerformerType.CreateAsync(performerTypeEntity);

                _response.Result = _mapper.Map<PerformerTypeDTO>(performerTypeEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPerformerType", new { id = performerTypeEntity.PerformerTypeId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePerformerType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePerformerType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var performerTypeEntity = await _dbPerformerType.GetAsync(u => u.PerformerTypeId == id);
                if (performerTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPerformerType.DeletePerformerTypeAsync(performerTypeEntity);

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
        [HttpPut("{id:int}", Name = "UpdatePerformerType")]
        public async Task<ActionResult<APIResponse>> UpdatePerformerType(int id, [FromBody] PerformerTypeDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.PerformerTypeId)
                {
                    return BadRequest();
                }

                PerformerType model = _mapper.Map<PerformerType>(updateDTO);

                await _dbPerformerType.UpdatePerformerTypeAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialPerformerType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialPerformerType(int id, JsonPatchDocument<PerformerTypeDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var performerTypeEntity = await _dbPerformerType.GetAsync(u => u.PerformerTypeId == id, tracked: false);

            if (performerTypeEntity == null)
            {
                return BadRequest();
            }

            PerformerTypeDTO performerTypeDTO = _mapper.Map<PerformerTypeDTO>(performerTypeEntity);
            patchDTO.ApplyTo(performerTypeDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PerformerType model = _mapper.Map<PerformerType>(performerTypeDTO);
            await _dbPerformerType.UpdatePerformerTypeAsync(model);

            return NoContent();
        }


    }
}
