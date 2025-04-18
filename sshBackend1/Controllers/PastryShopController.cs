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
    public class PastryShopController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPastryShopRepository _dbPastryShop;
        private readonly IMapper _mapper;

        public PastryShopController(IPastryShopRepository dbPastryShop, IMapper mapper)
        {
            _dbPastryShop = dbPastryShop;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPastryShops()
        {
            try
            {
                IEnumerable<PastryShop> pastryShopList = await _dbPastryShop.GetAllAsync();
                _response.Result = _mapper.Map<List<PastryShopDTO>>(pastryShopList);
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

        [HttpGet("{id:int}", Name = "GetPastryShop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPastryShop(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryShop = await _dbPastryShop.GetAsync(u => u.ShopId == id);
                if (pastryShop == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PastryShopDTO>(pastryShop);
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
        public async Task<ActionResult<APIResponse>> CreatePastryShop([FromBody] PastryShopDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid pastry shop data.");
                }

                if (await _dbPastryShop.GetAsync(u => u.ShopName.ToLower() == createDTO.ShopName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Pastry shop already exists!");
                    return BadRequest(ModelState);
                }

                PastryShop pastryShop = _mapper.Map<PastryShop>(createDTO);
                await _dbPastryShop.CreateAsync(pastryShop);

                _response.Result = _mapper.Map<PastryShopDTO>(pastryShop);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPastryShop", new { id = pastryShop.ShopId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePastryShop")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePastryShop(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var pastryShop = await _dbPastryShop.GetAsync(u => u.ShopId == id);
                if (pastryShop == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPastryShop.DeletePastryShopAsync(pastryShop);

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
        [HttpPut("{id:int}", Name = "UpdatePastryShop")]
        public async Task<ActionResult<APIResponse>> UpdatePastryShop(int id, [FromBody] PastryShopDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.ShopId)
                {
                    return BadRequest();
                }

                PastryShop model = _mapper.Map<PastryShop>(updateDTO);

                await _dbPastryShop.UpdatePastryShopAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialPastryShop")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialPastryShop(int id, JsonPatchDocument<PastryShopDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var pastryShop = await _dbPastryShop.GetAsync(u => u.ShopId == id, tracked: false);

            if (pastryShop == null)
            {
                return BadRequest();
            }

            PastryShopDTO pastryShopDTO = _mapper.Map<PastryShopDTO>(pastryShop);
            patchDTO.ApplyTo(pastryShopDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PastryShop model = _mapper.Map<PastryShop>(pastryShopDTO);
            await _dbPastryShop.UpdatePastryShopAsync(model);

            return NoContent();
        }
    }
}
