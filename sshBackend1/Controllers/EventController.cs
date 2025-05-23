using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class EventController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IEventRepository _dbEvent;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public EventController(IEventRepository dbEvent, IMapper mapper, ICacheService cacheService)
        {
            _dbEvent = dbEvent;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetEvents()
        {
            try
            {
                var eventList = await _cacheService.GetOrAddAsync("eventListCache",
                   async () => await _dbEvent.GetAllAsync(),
                   TimeSpan.FromMinutes(1));
                //IEnumerable<Event> eventList = await _dbEvent.GetAllAsync();
                _response.Result = _mapper.Map<List<EventDTO>>(eventList);
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

        [HttpGet("{id:int}", Name = "GetEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEvent(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var eventEntity = await _dbEvent.GetAsync(u => u.EventId == id);
                if (eventEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<EventDTO>(eventEntity);
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
        [Authorize(Roles = "client")]

        public async Task<ActionResult<APIResponse>> CreateEvent([FromBody] EventDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid event data.");
                }

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.IsSuccess = false;
                    _response.ErrorsMessages.Add("User not authorized.");
                    return Unauthorized(_response);
                }

                if (await _dbEvent.GetAsync(u => u.EventName.ToLower() == createDTO.EventName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Event already exists!");
                    return BadRequest(ModelState);
                }

                var eventEntity = _mapper.Map<Event>(createDTO);
                eventEntity.ApplicationUserId = userId; 

                await _dbEvent.CreateAsync(eventEntity);

                _response.Result = _mapper.Map<EventDTO>(eventEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetEvent", new { id = eventEntity.EventId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpDelete("{id:int}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteEvent(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var eventEntity = await _dbEvent.GetAsync(u => u.EventId == id);
                if (eventEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbEvent.DeleteEventAsync(eventEntity);

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
        [HttpPut("{id:int}", Name = "UpdateEvent")]
        public async Task<ActionResult<APIResponse>> UpdateEvent(int id, [FromBody] EventDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.EventId)
                {
                    return BadRequest();
                }

                Event model = _mapper.Map<Event>(updateDTO);

                await _dbEvent.UpdateEventAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialEvent(int id, JsonPatchDocument<EventDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var eventEntity = await _dbEvent.GetAsync(u => u.EventId == id, tracked: false);

            if (eventEntity == null)
            {
                return BadRequest();
            }

            EventDTO eventDTO = _mapper.Map<EventDTO>(eventEntity);
            patchDTO.ApplyTo(eventDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event model = _mapper.Map<Event>(eventDTO);
            await _dbEvent.UpdateEventAsync(model);

            return NoContent();
        }


        [Authorize(Roles = "CLIENT")]
        [HttpGet("mine")]
        public async Task<ActionResult<APIResponse>> GetMyEvents()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var events = await _dbEvent.GetAllAsync(e => e.ApplicationUserId == userId);

                _response.Result = _mapper.Map<List<EventDTO>>(events);
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



    }
}
