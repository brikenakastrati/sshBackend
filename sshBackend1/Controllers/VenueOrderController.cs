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
    public class VenueOrderController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVenueOrderRepository _dbVenueOrder;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public VenueOrderController(IVenueOrderRepository dbVenueOrder, IMapper mapper, ICacheService cacheService)
        {
            _dbVenueOrder = dbVenueOrder;
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
                var venueOrderList = await _cacheService.GetOrAddAsync("venueOrderListCache",
                    async () => await _dbVenueOrder.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<VenueOrder> venueOrderList = await _dbVenueOrder.GetAllAsync();
                _response.Result = _mapper.Map<List<VenueOrderDTO>>(venueOrderList);
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

        [HttpGet("{id:int}", Name = "GetVenueOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVenueOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var venueOrderEntity = await _dbVenueOrder.GetAsync(u => u.VenueOrderId == id);
                if (venueOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VenueOrderDTO>(venueOrderEntity);
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
        public async Task<ActionResult<APIResponse>> CreateEvent([FromBody] VenueOrderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Venue Order data.");
                }

                if (await _dbVenueOrder.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Venue Order already exists!");
                    return BadRequest(ModelState);
                }

                VenueOrder venueOrderEntity = _mapper.Map<VenueOrder>(createDTO);
                await _dbVenueOrder.CreateAsync(venueOrderEntity);

                _response.Result = _mapper.Map<VenueOrderDTO>(venueOrderEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVenueOrder", new { id = venueOrderEntity.VenueOrderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteVenueOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVenueOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var venueOrderEntity = await _dbVenueOrder.GetAsync(u => u.VenueOrderId == id);
                if (venueOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbVenueOrder.DeleteVenueOrderAsync(venueOrderEntity);

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
        [HttpPut("{id:int}", Name = "UpdateVenueOrder")]
        public async Task<ActionResult<APIResponse>> UpdateVenueOrder(int id, [FromBody] VenueOrderDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VenueOrderId)
                {
                    return BadRequest();
                }

                VenueOrder model = _mapper.Map<VenueOrder>(updateDTO);

                await _dbVenueOrder.UpdateVenueOrderAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialVenueOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVenueOrder(int id, JsonPatchDocument<VenueOrderDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var venueOrderEntity = await _dbVenueOrder.GetAsync(u => u.VenueOrderId == id, tracked: false);

            if (venueOrderEntity == null)
            {
                return BadRequest();
            }

            VenueOrderDTO venueOrderDTO = _mapper.Map<VenueOrderDTO>(venueOrderEntity);
            patchDTO.ApplyTo(venueOrderDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VenueOrder model = _mapper.Map<VenueOrder>(venueOrderDTO);
            await _dbVenueOrder.UpdateVenueOrderAsync(model);

            return NoContent();
        }


    }
}
