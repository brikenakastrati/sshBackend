using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PastryTypeController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPastryTypeRepository _dbPastryType;
        private readonly IMapper _mapper;

        public PastryTypeController(IPastryTypeRepository dbPastryType, IMapper mapper)
        {
            _dbPastryType = dbPastryType;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPastryTypes()
        {
            try
            {
                IEnumerable<PastryType> pastryTypeList = await _dbPastryType.GetAllAsync();
                _response.Result = _mapper.Map<List<PastryTypeDTO>>(pastryTypeList);
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

        [HttpGet("{id:int}", Name = "GetPastryType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPastryType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryType = await _dbPastryType.GetAsync(u => u.PastryTypeId == id);
                if (pastryType == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PastryTypeDTO>(pastryType);
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
        public async Task<ActionResult<APIResponse>> CreatePastryType([FromBody] PastryTypeDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid pastry type data.");
                }

                if (await _dbPastryType.GetAsync(u => u.TypeName.ToLower() == createDTO.TypeName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Pastry type already exists!");
                    return BadRequest(ModelState);
                }

                PastryType pastryType = _mapper.Map<PastryType>(createDTO);
                await _dbPastryType.CreateAsync(pastryType);

                _response.Result = _mapper.Map<PastryTypeDTO>(pastryType);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPastryType", new { id = pastryType.PastryTypeId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePastryType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePastryType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryType = await _dbPastryType.GetAsync(u => u.PastryTypeId == id);
                if (pastryType == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPastryType.DeletePastryTypeAsync(pastryType);

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
        [HttpPut("{id:int}", Name = "UpdatePastryType")]
        public async Task<ActionResult<APIResponse>> UpdatePastryType(int id, [FromBody] PastryTypeDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.PastryTypeId)
                {
                    return BadRequest();
                }

                PastryType model = _mapper.Map<PastryType>(updateDTO);

                await _dbPastryType.UpdatePastryTypeAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialPastryType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialPastryType(int id, JsonPatchDocument<PastryTypeDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var pastryType = await _dbPastryType.GetAsync(u => u.PastryTypeId == id, tracked: false);

            if (pastryType == null)
            {
                return BadRequest();
            }

            PastryTypeDTO pastryTypeDTO = _mapper.Map<PastryTypeDTO>(pastryType);
            patchDTO.ApplyTo(pastryTypeDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PastryType model = _mapper.Map<PastryType>(pastryTypeDTO);
            await _dbPastryType.UpdatePastryTypeAsync(model);

            return NoContent();
        }
    }
}
