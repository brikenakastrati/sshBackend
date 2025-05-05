using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
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
    public class VenueController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVenueRepository _dbVenue;
        protected APIResponse _response;
        private readonly ICacheService _cacheService;

        public VenueController(IVenueRepository dbVenue, IMapper mapper, ICacheService cacheService)
        {
            _dbVenue = dbVenue;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllVenues()
        {
            try
            {
                var venueList = await _cacheService.GetOrAddAsync("venueListCache", 
                    async () => await _dbVenue.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<Venue> venueList = await _dbVenue.GetAllAsync();
                _response.Result = _mapper.Map<List<VenueDTO>>(venueList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{id:int}", Name = "GetVenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVenue(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                var venueEntity = await _dbVenue.GetAsync(v => v.VenueId == id);
                if (venueEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.Result = _mapper.Map<VenueDTO>(venueEntity);
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVenue([FromBody] VenueDTO createDTO)
        {
            try
            { 
                if (createDTO == null)
                {
                    return BadRequest("Invalid Data");
                }
                if (await _dbVenue.GetAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Venue already exists!");
                    return BadRequest(ModelState);
                }
               
                Venue venueEntity = _mapper.Map<Venue>(createDTO);
                await _dbVenue.CreateAsync(venueEntity);
                _response.Result = venueEntity;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVenue", new { id = venueEntity.VenueId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateVenue")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVenue(int id, [FromBody] VenueDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VenueId)
                {
                    return BadRequest();
                }
                Venue model = _mapper.Map<Venue>(updateDTO);

                await _dbVenue.UpdateVenueAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteVenue")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVenue(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var venueEntity = await _dbVenue.GetAsync(v => v.VenueId == id);
                if (venueEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _dbVenue.DeleteVenueAsync(venueEntity);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVenue")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVenue(int id, JsonPatchDocument<VenueDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var venueEntity = await _dbVenue.GetAsync(u => u.VenueId == id, tracked: false);

            if (venueEntity == null)
            {
                return BadRequest();
            }

            VenueDTO venueDTO = _mapper.Map<VenueDTO>(venueEntity);
            patchDTO.ApplyTo(venueDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Venue model = _mapper.Map<Venue>(venueDTO);
            await _dbVenue.UpdateVenueAsync(model);

            return NoContent();
        }

    }
}
