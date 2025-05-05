using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Models.DTOs;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;
using System.Net;
using sshBackend1.Services.IServices;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IOrderStatusRepository _dbOrderStatus;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;


        public OrderStatusController(IOrderStatusRepository dbOrderStatus, IMapper mapper, ICacheService cacheService)
        {
            _dbOrderStatus = dbOrderStatus;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetOrderStatuses()
        {
            try
            {
                var orderStatusList = await _cacheService.GetOrAddAsync("orderStatusListCache",
                    async () => await _dbOrderStatus.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<OrderStatus> orderStatusList = await _dbOrderStatus.GetAllAsync();
                _response.Result = _mapper.Map<List<OrderStatusDTO>>(orderStatusList);
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

        [HttpGet("{id:int}", Name = "GetOrderStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrderStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var orderStatusEntity = await _dbOrderStatus.GetAsync(u => u.OrderStatusId == id);
                if (orderStatusEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<OrderStatusDTO>(orderStatusEntity);
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
        public async Task<ActionResult<APIResponse>> CreateOrderStatus([FromBody] OrderStatusDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Order Status data.");
                }

                if (await _dbOrderStatus.GetAsync(u => u.OrderStatusName.ToLower() == createDTO.OrderStatusName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Order Status already exists!");
                    return BadRequest(ModelState);
                }

                OrderStatus orderStatusEntity = _mapper.Map<OrderStatus>(createDTO);
                await _dbOrderStatus.CreateAsync(orderStatusEntity);

                _response.Result = _mapper.Map<OrderStatusDTO>(orderStatusEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetOrderStatus", new { id = orderStatusEntity.OrderStatusId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteOrderStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteOrderStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var orderStatusEntity = await _dbOrderStatus.GetAsync(u => u.OrderStatusId == id);
                if (orderStatusEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbOrderStatus.DeleteOrderStatusAsync(orderStatusEntity);

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
        [HttpPut("{id:int}", Name = "UpdateOrderStatus")]
        public async Task<ActionResult<APIResponse>> UpdateOrderStatus(int id, [FromBody] OrderStatusDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.OrderStatusId)
                {
                    return BadRequest();
                }

                OrderStatus model = _mapper.Map<OrderStatus>(updateDTO);

                await _dbOrderStatus.UpdateOrderStatusAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialOrderStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialOrderStatus(int id, JsonPatchDocument<OrderStatusDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var orderStatusEntity = await _dbOrderStatus.GetAsync(u => u.OrderStatusId == id, tracked: false);

            if (orderStatusEntity == null)
            {
                return BadRequest();
            }

            OrderStatusDTO orderStatusDTO = _mapper.Map<OrderStatusDTO>(orderStatusEntity);
            patchDTO.ApplyTo(orderStatusDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OrderStatus model = _mapper.Map<OrderStatus>(orderStatusDTO);
            await _dbOrderStatus.UpdateOrderStatusAsync(model);

            return NoContent();
        }
    }
}
