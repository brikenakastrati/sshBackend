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
    public class FlowerArrangementOrderController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IFlowerArrangementOrderRepository _dbFlowerArrangementOrder;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;


        public FlowerArrangementOrderController(IFlowerArrangementOrderRepository dbFlowerArrangementOrder, IMapper mapper, ICacheService cacheService)
        {
            _dbFlowerArrangementOrder = dbFlowerArrangementOrder;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetFlowerArrangementOrders()
        {
            try
            {
                var flowerArrangementOrdersList = await _cacheService.GetOrAddAsync("flowerArrangementOrdersListCache",
                    async () => await _dbFlowerArrangementOrder.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<FlowerArrangementOrder> flowerArrangementOrdersList = await _dbFlowerArrangementOrder.GetAllAsync();
                _response.Result = _mapper.Map<List<FlowerArrangementOrderDTO>>(flowerArrangementOrdersList);
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

        [HttpGet("{id:int}", Name = "GetFlowerArrangementOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetFlowerArrangementOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var flowerArrangementOrderEntity = await _dbFlowerArrangementOrder.GetAsync(u => u.FlowerArrangementOrderId == id);
                if (flowerArrangementOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<FlowerArrangementOrderDTO>(flowerArrangementOrderEntity);
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
        public async Task<ActionResult<APIResponse>> CreateFlowerArrangementOrder([FromBody] FlowerArrangementOrderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Flower Arrangement Order data.");
                }

                if (await _dbFlowerArrangementOrder.GetAsync(u => u.Name.ToLower() == createDTO.OrderName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Flower Arrangement Order already exists!");
                    return BadRequest(ModelState);
                }

                FlowerArrangementOrder flowerArrangementOrderEntity = _mapper.Map<FlowerArrangementOrder>(createDTO);
                await _dbFlowerArrangementOrder.CreateAsync(flowerArrangementOrderEntity);

                _response.Result = _mapper.Map<FlowerArrangementOrderDTO>(flowerArrangementOrderEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetFlowerArrangementOrder", new { id = flowerArrangementOrderEntity.FlowerArrangementOrderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteFlowerArrangementOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteFlowerArrangementOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var flowerArrangementOrderEntity = await _dbFlowerArrangementOrder.GetAsync(u => u.FlowerArrangementOrderId == id);
                if (flowerArrangementOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbFlowerArrangementOrder.DeleteFlowerArrangementOrderAsync(flowerArrangementOrderEntity);

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
        [HttpPut("{id:int}", Name = "UpdateFlowerArrangementOrder")]
        public async Task<ActionResult<APIResponse>> UpdateFlowerArrangementOrder(int id, [FromBody] FlowerArrangementOrderDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.FlowerArrangementOrderId)
                {
                    return BadRequest();
                }

                FlowerArrangementOrder model = _mapper.Map<FlowerArrangementOrder>(updateDTO);

                await _dbFlowerArrangementOrder.UpdateFlowerArrangementOrderAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialFlowerArrangementOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialFlowerArrangementOrder(int id, JsonPatchDocument<FlowerArrangementOrderDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var flowerArrangementOrderEntity = await _dbFlowerArrangementOrder.GetAsync(u => u.FlowerArrangementOrderId == id, tracked: false);

            if (flowerArrangementOrderEntity == null)
            {
                return BadRequest();
            }

            FlowerArrangementOrderDTO flowerArrangementOrderDTO = _mapper.Map<FlowerArrangementOrderDTO>(flowerArrangementOrderEntity);
            patchDTO.ApplyTo(flowerArrangementOrderDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FlowerArrangementOrder model = _mapper.Map<FlowerArrangementOrder>(flowerArrangementOrderDTO);
            await _dbFlowerArrangementOrder.UpdateFlowerArrangementOrderAsync(model);

            return NoContent();
        }
    }
}
