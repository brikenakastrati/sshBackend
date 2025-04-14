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
    public class RestaurantStatusController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRestaurantStatusRepository _dbRestaurantStatus;
        private readonly IMapper _mapper;

        public RestaurantStatusController(IRestaurantStatusRepository dbRestaurantStatus, IMapper mapper)
        {
            _dbRestaurantStatus = dbRestaurantStatus;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetRestaurantStatuses()
        {
            try
            {
                IEnumerable<RestaurantStatus> restaurantStatusList = await _dbRestaurantStatus.GetAllAsync();
                _response.Result = _mapper.Map<List<RestaurantStatusDTO>>(restaurantStatusList);
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

        [HttpGet("{id:int}", Name = "GetRestaurantStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRestaurantStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var restaurantStatusEntity = await _dbRestaurantStatus.GetAsync(u => u.RestaurantStatusId == id);
                if (restaurantStatusEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<RestaurantStatusDTO>(restaurantStatusEntity);
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
        public async Task<ActionResult<APIResponse>> CreateRestaurantStatus([FromBody] RestaurantStatusDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Restaurant Status data.");
                }

                if (await _dbRestaurantStatus.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Restaurant Status already exists!");
                    return BadRequest(ModelState);
                }

                RestaurantStatus restaurantStatusEntity = _mapper.Map<RestaurantStatus>(createDTO);
                await _dbRestaurantStatus.CreateAsync(restaurantStatusEntity);

                _response.Result = _mapper.Map<RestaurantStatusDTO>(restaurantStatusEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetRestaurantStatus", new { id = restaurantStatusEntity.RestaurantStatusId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteRestaurantStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteRestaurantStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var restaurantStatusEntity = await _dbRestaurantStatus.GetAsync(u => u.RestaurantStatusId == id);
                if (restaurantStatusEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbRestaurantStatus.DeleteRestaurantStatusAsync(restaurantStatusEntity);

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
        [HttpPut("{id:int}", Name = "UpdateRestaurantStatus")]
        public async Task<ActionResult<APIResponse>> UpdateRestaurantStatus(int id, [FromBody] RestaurantStatusDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.RestaurantStatusId)
                {
                    return BadRequest();
                }

                RestaurantStatus model = _mapper.Map<RestaurantStatus>(updateDTO);

                await _dbRestaurantStatus.UpdateRestaurantStatusAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialRestaurantStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialRestaurantStatus(int id, JsonPatchDocument<RestaurantStatusDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var restaurantStatusEntity = await _dbRestaurantStatus.GetAsync(u => u.RestaurantStatusId == id, tracked: false);

            if (restaurantStatusEntity == null)
            {
                return BadRequest();
            }

            RestaurantStatusDTO restaurantStatusDTO = _mapper.Map<RestaurantStatusDTO>(restaurantStatusEntity);
            patchDTO.ApplyTo(restaurantStatusDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RestaurantStatus model = _mapper.Map<RestaurantStatus>(restaurantStatusDTO);
            await _dbRestaurantStatus.UpdateRestaurantStatusAsync(model);

            return NoContent();
        }


    }
}
