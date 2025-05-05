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
    public class GuestController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGuestRepository _dbGuest;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GuestController(IGuestRepository dbGuest, IMapper mapper, ICacheService cacheService)
        {
            _dbGuest = dbGuest;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetGuests()
        {
            try
            {
                var guestList = await _cacheService.GetOrAddAsync("guestListCache",
                    async () => await _dbGuest.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<Guest> guestList = await _dbGuest.GetAllAsync();
                _response.Result = _mapper.Map<List<GuestDTO>>(guestList);
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

        [HttpGet("{id:int}", Name = "GetGuest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetGuest(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var guestEntity = await _dbGuest.GetAsync(u => u.GuestId == id);
                if (guestEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<GuestDTO>(guestEntity);
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
        public async Task<ActionResult<APIResponse>> CreateGuest([FromBody] GuestDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Guest data.");
                }

                if (await _dbGuest.GetAsync(u => u.GuestName.ToLower() == createDTO.GuestName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Guest already exists!");
                    return BadRequest(ModelState);
                }

                Guest guestEntity = _mapper.Map<Guest>(createDTO);
                await _dbGuest.CreateAsync(guestEntity);

                _response.Result = _mapper.Map<GuestDTO>(guestEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetGuest", new { id = guestEntity.EventId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteGuest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteGuest(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var guestEntity = await _dbGuest.GetAsync(u => u.GuestId == id);
                if (guestEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbGuest.DeleteGuestAsync(guestEntity);

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
        [HttpPut("{id:int}", Name = "UpdateGuest")]
        public async Task<ActionResult<APIResponse>> UpdateGuest(int id, [FromBody] GuestDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.GuestId)
                {
                    return BadRequest();
                }

                Guest model = _mapper.Map<Guest>(updateDTO);

                await _dbGuest.UpdateGuestAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialGuest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialGuest(int id, JsonPatchDocument<GuestDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var guestEntity = await _dbGuest.GetAsync(u => u.GuestId == id, tracked: false);

            if (guestEntity == null)
            {
                return BadRequest();
            }

            GuestDTO guestDTO = _mapper.Map<GuestDTO>(guestEntity);
            patchDTO.ApplyTo(guestDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guest model = _mapper.Map<Guest>(guestDTO);
            await _dbGuest.UpdateGuestAsync(model);

            return NoContent();
        }


    }
}
