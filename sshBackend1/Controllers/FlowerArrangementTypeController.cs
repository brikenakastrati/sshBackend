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
    public class FlowerArrangementTypeController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IFlowerArrangementTypeRepository _dbFlowerArrangementType;
        private readonly IMapper _mapper;

        public FlowerArrangementTypeController(IFlowerArrangementTypeRepository dbFlowerArrangementType, IMapper mapper)
        {
            _dbFlowerArrangementType = dbFlowerArrangementType;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetFlowerArrangementTypes()
        {
            try
            {
                IEnumerable<FlowerArrangementType> flowerArrangementTypeList = await _dbFlowerArrangementType.GetAllAsync();
                _response.Result = _mapper.Map<List<FlowerArrangementTypeDTO>>(flowerArrangementTypeList);
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

        [HttpGet("{id:int}", Name = "GetFlowerArrangementType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetFlowerArrangementType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var flowerArrangementTypeEntity = await _dbFlowerArrangementType.GetAsync(u => u.FlowerArrangementTypeId == id);
                if (flowerArrangementTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<FlowerArrangementTypeDTO>(flowerArrangementTypeEntity);
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
        public async Task<ActionResult<APIResponse>> CreateFlowerArrangementType([FromBody] FlowerArrangementTypeDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Flower Arrangement Type data.");
                }

                if (await _dbFlowerArrangementType.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Flower Arrangement Type already exists!");
                    return BadRequest(ModelState);
                }

                FlowerArrangementType flowerArrangementTypeEntity = _mapper.Map<FlowerArrangementType>(createDTO);
                await _dbFlowerArrangementType.CreateAsync(flowerArrangementTypeEntity);

                _response.Result = _mapper.Map<FlowerArrangementTypeDTO>(flowerArrangementTypeEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetFlowerArrangementType", new { id = flowerArrangementTypeEntity.FlowerArrangementTypeId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteFlowerArrangementType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteFlowerArrangementType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var flowerArrangementTypeEntity = await _dbFlowerArrangementType.GetAsync(u => u.FlowerArrangementTypeId == id);
                if (flowerArrangementTypeEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbFlowerArrangementType.DeleteFlowerArrangementTypesAsync(flowerArrangementTypeEntity);

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
        [HttpPut("{id:int}", Name = "UpdateDeleteFlowerArrangementType")]
        public async Task<ActionResult<APIResponse>> UpdateDeleteFlowerArrangementType(int id, [FromBody] FlowerArrangementTypeDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.FlowerArrangementTypeId)
                {
                    return BadRequest();
                }

                FlowerArrangementType model = _mapper.Map<FlowerArrangementType>(updateDTO);

                await _dbFlowerArrangementType.UpdateFlowerArrangementTypeAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialFlowerArrangementType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialFlowerArrangementType(int id, JsonPatchDocument<FlowerArrangementTypeDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var flowerArrangementTypeEntity = await _dbFlowerArrangementType.GetAsync(u => u.FlowerArrangementTypeId == id, tracked: false);

            if (flowerArrangementTypeEntity == null)
            {
                return BadRequest();
            }

            FlowerArrangementTypeDTO flowerArrangementTypeDTO = _mapper.Map<FlowerArrangementTypeDTO>(flowerArrangementTypeEntity);
            patchDTO.ApplyTo(flowerArrangementTypeDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FlowerArrangementType model = _mapper.Map<FlowerArrangementType>(flowerArrangementTypeDTO);
            await _dbFlowerArrangementType.UpdateFlowerArrangementTypeAsync(model);

            return NoContent();
        }


    }
}
