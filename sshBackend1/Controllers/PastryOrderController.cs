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
    public class PastryOrderController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPastryOrderRepository _dbPastryOrder;
        private readonly IMapper _mapper;

        public PastryOrderController(IPastryOrderRepository dbPastryOrder, IMapper mapper)
        {
            _dbPastryOrder = dbPastryOrder;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPastryOrders()
        {
            try
            {
                IEnumerable<PastryOrder> pastryOrderList = await _dbPastryOrder.GetAllAsync();
                _response.Result = _mapper.Map<List<PastryOrderDTO>>(pastryOrderList);
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

        [HttpGet("{id:int}", Name = "GetPastryOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPastryOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryOrderEntity = await _dbPastryOrder.GetAsync(u => u.PastryOrderId == id);
                if (pastryOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PastryOrderDTO>(pastryOrderEntity);
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
        public async Task<ActionResult<APIResponse>> CreatePastryOrder([FromBody] PastryOrderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid pastry order data.");
                }

                if (await _dbPastryOrder.GetAsync(u => u.Name.ToLower() == createDTO.OrderName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Pastry order already exists!");
                    return BadRequest(ModelState);
                }

                PastryOrder pastryOrderEntity = _mapper.Map<PastryOrder>(createDTO);
                await _dbPastryOrder.CreateAsync(pastryOrderEntity);

                _response.Result = _mapper.Map<PastryOrderDTO>(pastryOrderEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPastryOrder", new { id = pastryOrderEntity.PastryOrderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePastryOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePastryOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryOrderEntity = await _dbPastryOrder.GetAsync(u => u.PastryOrderId == id);
                if (pastryOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPastryOrder.DeletePastryOrderAsync(pastryOrderEntity);

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
        [HttpPut("{id:int}", Name = "UpdatePastryOrder")]
        public async Task<ActionResult<APIResponse>> UpdatePastryOrder(int id, [FromBody] PastryOrderDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.PastryOrderId)
                {
                    return BadRequest();
                }

                PastryOrder model = _mapper.Map<PastryOrder>(updateDTO);

                await _dbPastryOrder.UpdatePastryOrderAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialPastryOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialPastryOrder(int id, JsonPatchDocument<PastryOrderDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var pastryOrderEntity = await _dbPastryOrder.GetAsync(u => u.PastryOrderId == id, tracked: false);

            if (pastryOrderEntity == null)
            {
                return BadRequest();
            }

            PastryOrderDTO pastryOrderDTO = _mapper.Map<PastryOrderDTO>(pastryOrderEntity);
            patchDTO.ApplyTo(pastryOrderDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PastryOrder model = _mapper.Map<PastryOrder>(pastryOrderDTO);
            await _dbPastryOrder.UpdatePastryOrderAsync(model);

            return NoContent();
        }
    }
}
