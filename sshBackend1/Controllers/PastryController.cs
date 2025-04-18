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
    public class PastryController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPastryRepository _dbPastry;
        private readonly IMapper _mapper;

        public PastryController(IPastryRepository dbPastry, IMapper mapper)
        {
            _dbPastry = dbPastry;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPastries()
        {
            try
            {
                IEnumerable<Pastry> pastryList = await _dbPastry.GetAllAsync();
                _response.Result = _mapper.Map<List<PastryDTO>>(pastryList);
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

        [HttpGet("{id:int}", Name = "GetPastry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPastry(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryEntity = await _dbPastry.GetAsync(u => u.PastryId == id);
                if (pastryEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PastryDTO>(pastryEntity);
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
        public async Task<ActionResult<APIResponse>> CreatePastry([FromBody] PastryDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid pastry data.");
                }

                if (await _dbPastry.GetAsync(u => u.PastryName.ToLower() == createDTO.PastryName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Pastry already exists!");
                    return BadRequest(ModelState);
                }

                Pastry pastryEntity = _mapper.Map<Pastry>(createDTO);
                await _dbPastry.CreateAsync(pastryEntity);

                _response.Result = _mapper.Map<PastryDTO>(pastryEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPastry", new { id = pastryEntity.PastryId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePastry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePastry(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryEntity = await _dbPastry.GetAsync(u => u.PastryId == id);
                if (pastryEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPastry.DeletePastryAsync(pastryEntity);

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
        [HttpPut("{id:int}", Name = "UpdatePastry")]
        public async Task<ActionResult<APIResponse>> UpdatePastry(int id, [FromBody] PastryDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.PastryId)
                {
                    return BadRequest();
                }

                Pastry model = _mapper.Map<Pastry>(updateDTO);

                await _dbPastry.UpdatePastryAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialPastry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialPastry(int id, JsonPatchDocument<PastryDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var pastryEntity = await _dbPastry.GetAsync(u => u.PastryId == id, tracked: false);

            if (pastryEntity == null)
            {
                return BadRequest();
            }

            PastryDTO pastryDTO = _mapper.Map<PastryDTO>(pastryEntity);
            patchDTO.ApplyTo(pastryDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pastry model = _mapper.Map<Pastry>(pastryDTO);
            await _dbPastry.UpdatePastryAsync(model);

            return NoContent();
        }
    }
}
