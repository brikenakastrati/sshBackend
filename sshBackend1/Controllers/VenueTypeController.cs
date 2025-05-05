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
    public class VenueTypeController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVenueTypeRepository _dbVenueType;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public VenueTypeController(IVenueTypeRepository dbVenueType, IMapper mapper, ICacheService cacheService)
        {
            _dbVenueType = dbVenueType;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVenueTypes()
        {
            try
            {
                var venueTypeList = await _cacheService.GetOrAddAsync("venueTypeList",
                    async () => await _dbVenueType.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<VenueType> venueTypeList = await _dbVenueType.GetAllAsync();
                _response.Result = _mapper.Map<List<VenueTypeDTO>>(venueTypeList);
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

        [HttpGet("{id:int}", Name = "GetVenueType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVenueType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var venueTypeEntity = await _dbVenueType.GetAsync(u => u.VenueTypeId == id);
                if (venueTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VenueTypeDTO>(venueTypeEntity);
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
        public async Task<ActionResult<APIResponse>> CreateVenueType([FromBody] VenueTypeDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Venue Type data.");
                }

                if (await _dbVenueType.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Venue Type already exists!");
                    return BadRequest(ModelState);
                }

                VenueType venueTypeEntity = _mapper.Map<VenueType>(createDTO);
                await _dbVenueType.CreateAsync(venueTypeEntity);

                _response.Result = _mapper.Map<VenueTypeDTO>(venueTypeEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVenueType", new { id = venueTypeEntity.VenueTypeId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteVenueType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVenueType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var venueTypeEntity = await _dbVenueType.GetAsync(u => u.VenueTypeId == id);
                if (venueTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbVenueType.DeleteVenueTypeAsync(venueTypeEntity);

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
        [HttpPut("{id:int}", Name = "UpdateVenueType")]
        public async Task<ActionResult<APIResponse>> UpdateVenueType(int id, [FromBody] VenueTypeDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VenueTypeId)
                {
                    return BadRequest();
                }

                VenueType model = _mapper.Map<VenueType>(updateDTO);

                await _dbVenueType.UpdateVenueTypeAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialVenueType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVenueType(int id, JsonPatchDocument<VenueTypeDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var venueTypeEntity = await _dbVenueType.GetAsync(u => u.VenueTypeId == id, tracked: false);

            if (venueTypeEntity == null)
            {
                return BadRequest();
            }

            VenueTypeDTO venueTypeDTO = _mapper.Map<VenueTypeDTO>(venueTypeEntity);
            patchDTO.ApplyTo(venueTypeDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VenueType model = _mapper.Map<VenueType>(venueTypeDTO);
            await _dbVenueType.UpdateVenueTypeAsync(model);

            return NoContent();
        }


    }
}
