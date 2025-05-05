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
    public class RestaurantController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRestaurantRepository _dbRestaurant;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public RestaurantController(IRestaurantRepository dbRestaurant, IMapper mapper, ICacheService cacheService)
        {
            _dbRestaurant = dbRestaurant;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetRestaurants()
        {
            try
            {
                var restaurantList = await _cacheService.GetOrAddAsync("restaurantListCache",
                    async () => await _dbRestaurant.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<Restaurant> restaurantList = await _dbRestaurant.GetAllAsync();
                _response.Result = _mapper.Map<List<RestaurantDTO>>(restaurantList);
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

        [HttpGet("{id:int}", Name = "GetRestaurant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRestaurant(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var restaurantEntity = await _dbRestaurant.GetAsync(u => u.RestaurantId == id);
                if (restaurantEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<RestaurantDTO>(restaurantEntity);
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
        public async Task<ActionResult<APIResponse>> CreateRestaurant([FromBody] RestaurantDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Restaurant data.");
                }

                if (await _dbRestaurant.GetAsync(u => u.RestaurantName.ToLower() == createDTO.RestaurantName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Restaurant already exists!");
                    return BadRequest(ModelState);
                }

                Restaurant restaurantEntity = _mapper.Map<Restaurant>(createDTO);
                await _dbRestaurant.CreateAsync(restaurantEntity);

                _response.Result = _mapper.Map<RestaurantDTO>(restaurantEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetRestaurant", new { id = restaurantEntity.RestaurantId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteRestaurant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteRestaurant(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var restaurantEntity = await _dbRestaurant.GetAsync(u => u.RestaurantId == id);
                if (restaurantEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbRestaurant.DeleteRestaurantAsync(restaurantEntity);

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
        [HttpPut("{id:int}", Name = "UpdateRestaurant")]
        public async Task<ActionResult<APIResponse>> UpdateRestaurant(int id, [FromBody] RestaurantDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.RestaurantId)
                {
                    return BadRequest();
                }

                Restaurant model = _mapper.Map<Restaurant>(updateDTO);

                await _dbRestaurant.UpdateRestaurantAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialRestaurant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialRestaurant(int id, JsonPatchDocument<RestaurantDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();

            }

            var restaurant = await _dbRestaurant.GetAsync(u => u.RestaurantId == id, tracked: false);

            RestaurantDTO restaurantDTO = _mapper.Map<RestaurantDTO>(restaurant);

            if (restaurant == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(restaurantDTO, ModelState);
            Restaurant model = _mapper.Map<Restaurant>(restaurantDTO);

            await _dbRestaurant.UpdateRestaurantAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
