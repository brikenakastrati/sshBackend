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

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueProviderController : ControllerBase
    {

        protected APIResponse _response;
        private readonly IVenueProviderRepository _dbVenueProvider;
        private readonly IMapper _mapper;

        public VenueProviderController(IVenueProviderRepository dbVenueProvider, IMapper mapper)
        {
            _dbVenueProvider = dbVenueProvider;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVenueProviders()
        {
            try
            {
                IEnumerable<VenueProvider> venueProviderList = await _dbVenueProvider.GetAllAsync();
                _response.Result = _mapper.Map<List<VenueProviderDTO>>(venueProviderList);
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

        [HttpGet("{id:int}", Name = "GetVenueProvider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVenueProvider(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var VenueProviderEntity = await _dbVenueProvider.GetAsync(u => u.VenueProviderId == id);
                if (VenueProviderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VenueProviderDTO > (VenueProviderEntity);
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
        public async Task<ActionResult<APIResponse>> CreateVenueProvider([FromBody] VenueProviderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Venue Provider data.");
                }

                if (await _dbVenueProvider.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Venue Provider already exists!");
                    return BadRequest(ModelState);
                }

                VenueProvider venueProviderEntity = _mapper.Map<VenueProvider>(createDTO);
                await _dbVenueProvider.CreateAsync(venueProviderEntity);

                _response.Result = _mapper.Map<VenueProviderDTO>(venueProviderEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVenueProvider", new { id = venueProviderEntity.VenueProviderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteVenueProvider")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVenueProvider(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var venueProviderEntity = await _dbVenueProvider.GetAsync(u => u.VenueProviderId == id);
                if (venueProviderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbVenueProvider.DeleteVenueProviderAsync(venueProviderEntity);

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
        [HttpPut("{id:int}", Name = "UpdateVenueProvider")]
        public async Task<ActionResult<APIResponse>> UpdateVenueProvider(int id, [FromBody] VenueProviderDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VenueProviderId)
                {
                    return BadRequest();
                }

                VenueProvider model = _mapper.Map<VenueProvider>(updateDTO);

                await _dbVenueProvider.UpdateVenueProviderAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialVenueProvider")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVenueProvider(int id, JsonPatchDocument<VenueProviderDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();

            }

            var venueProvider = await _dbVenueProvider.GetAsync(u => u.VenueProviderId == id, tracked: false);

            VenueProviderDTO venueProviderDTO = _mapper.Map<VenueProviderDTO>(venueProvider);

            if (venueProvider == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(venueProviderDTO, ModelState);
            VenueProvider model = _mapper.Map<VenueProvider>(venueProviderDTO);

            await _dbVenueProvider.UpdateVenueProviderAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
