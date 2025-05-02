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
    public class FlowerArrangementController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IFlowerArrangementRepository _dbFlowerArrangement;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FlowerArrangementController(IFlowerArrangementRepository dbFlowerArrangement, IMapper mapper, ICacheService cacheService)
        {
            _dbFlowerArrangement = dbFlowerArrangement;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetFlowerArrangements()
        {
            try
            {
                var flowerArrangementList = await _cacheService.GetOrAddAsync("flowerArrangementCache",
                    async () => await _dbFlowerArrangement.GetAllAsync(),
                    TimeSpan.FromMinutes(1));
                //IEnumerable<FlowerArrangement> flowerArrangementList = await _dbFlowerArrangement.GetAllAsync();
                _response.Result = _mapper.Map<List<FlowerArrangementDTO>>(flowerArrangementList);
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

        [HttpGet("{id:int}", Name = "GetFlowerArrangement")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetFlowerArrangement(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var flowerArrangementEntity = _dbFlowerArrangement.GetAsync(u => u.FlowerArrangementId == id);
                if (flowerArrangementEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<FlowerArrangementDTO>(flowerArrangementEntity);
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
        public async Task<ActionResult<APIResponse>> CreateFlowerArrangement([FromBody] FlowerArrangementDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Flower Arrangement data.");
                }

                if (await _dbFlowerArrangement.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Flower Arrangement already exists!");
                    return BadRequest(ModelState);
                }

                FlowerArrangement flowerArrangementEntity = _mapper.Map<FlowerArrangement>(createDTO);
                await _dbFlowerArrangement.CreateAsync(flowerArrangementEntity);

                _response.Result = _mapper.Map<FlowerArrangementDTO>(flowerArrangementEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetFlowerArrangement", new { id = flowerArrangementEntity.FlowerArrangementId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteFlowerArrangement")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteFlowerArrangement(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var flowerArrangementEntity = await _dbFlowerArrangement.GetAsync(u => u.FlowerArrangementId == id);
                if (flowerArrangementEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbFlowerArrangement.DeleteFlowerArrangementAsync(flowerArrangementEntity);

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
        [HttpPut("{id:int}", Name = "UpdateFlowerArrangement")]
        public async Task<ActionResult<APIResponse>> UpdateFlowerArrangement(int id, [FromBody] FlowerArrangementDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.FlowerArrangementId)
                {
                    return BadRequest();
                }

                FlowerArrangement model = _mapper.Map<FlowerArrangement>(updateDTO);

                await _dbFlowerArrangement.UpdateFlowerArrangementAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialFlowerArrangement")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialFlowerArrangement(int id, JsonPatchDocument<FlowerArrangementDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var flowerArrangementEntity = await _dbFlowerArrangement.GetAsync(u => u.FlowerArrangementId == id, tracked: false);

            if (flowerArrangementEntity == null)
            {
                return BadRequest();
            }

            FlowerArrangementDTO flowerArrangementDTO = _mapper.Map<FlowerArrangementDTO>(flowerArrangementEntity);
            patchDTO.ApplyTo(flowerArrangementDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FlowerArrangement model = _mapper.Map<FlowerArrangement>(flowerArrangementDTO);
            await _dbFlowerArrangement.UpdateFlowerArrangementAsync(model);

            return NoContent();
        }
    }
}
