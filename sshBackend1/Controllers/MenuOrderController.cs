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
    public class MenuOrderController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMenuOrderRepository _dbMenuOrder;
        private readonly IMapper _mapper;

        public MenuOrderController(IMenuOrderRepository dbMenuOrder, IMapper mapper)
        {
            _dbMenuOrder = dbMenuOrder;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMenuOrders()
        {
            try
            {
                IEnumerable<MenuOrder> menuOrderList = await _dbMenuOrder.GetAllAsync();
                _response.Result = _mapper.Map<List<MenuOrderDTO>>(menuOrderList);
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

        [HttpGet("{id:int}", Name = "GetMenuOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMenuOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var menuOrderEntity = await _dbMenuOrder.GetAsync(u => u.MenuOrderId == id);
                if (menuOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<MenuOrderDTO>(menuOrderEntity);
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
        public async Task<ActionResult<APIResponse>> CreateMenuOrder([FromBody] MenuOrderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Menu Order data.");
                }

                if (await _dbMenuOrder.GetAsync(u => u.OrderName.ToLower() == createDTO.OrderName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Menu Order already exists!");
                    return BadRequest(ModelState);
                }

                MenuOrder menuOrderEntity = _mapper.Map<MenuOrder>(createDTO);
                await _dbMenuOrder.CreateAsync(menuOrderEntity);

                _response.Result = _mapper.Map<MenuOrderDTO>(menuOrderEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetMenuOrder", new { id = menuOrderEntity.MenuOrderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteMenuOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteMenuOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var menuOrderEntity = await _dbMenuOrder.GetAsync(u => u.MenuOrderId == id);
                if (menuOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbMenuOrder.DeleteMenuOrderAsync(menuOrderEntity);

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
        [HttpPut("{id:int}", Name = "UpdateMenuOrder")]
        public async Task<ActionResult<APIResponse>> UpdateMenuOrder(int id, [FromBody] MenuOrderDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.MenuOrderId)
                {
                    return BadRequest();
                }

                MenuOrder model = _mapper.Map<MenuOrder>(updateDTO);

                await _dbMenuOrder.UpdateMenuOrderAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialMenuOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialMenuOrder(int id, JsonPatchDocument<MenuOrderDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var menuOrderEntity = await _dbMenuOrder.GetAsync(u => u.MenuOrderId == id, tracked: false);

            if (menuOrderEntity == null)
            {
                return BadRequest();
            }

            MenuOrderDTO menuOrderDTO = _mapper.Map<MenuOrderDTO>(menuOrderEntity);
            patchDTO.ApplyTo(menuOrderDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MenuOrder model = _mapper.Map<MenuOrder>(menuOrderDTO);
            await _dbMenuOrder.UpdateMenuOrderAsync(model);

            return NoContent();
        }
    }
}
