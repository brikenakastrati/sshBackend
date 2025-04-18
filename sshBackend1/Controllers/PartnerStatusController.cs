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
    public class PartnerStatusController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPartnerStatusRepository _dbPartnerStatus;
        private readonly IMapper _mapper;

        public PartnerStatusController(IPartnerStatusRepository dbPartnerStatus, IMapper mapper)
        {
            _dbPartnerStatus = dbPartnerStatus;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPartnerStatuses()
        {
            try
            {
                IEnumerable<PartnerStatus> partnerStatusList = await _dbPartnerStatus.GetAllAsync();
                _response.Result = _mapper.Map<List<PartnerStatusDTO>>(partnerStatusList);
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

        [HttpGet("{id:int}", Name = "GetPartnerStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPartnerStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var partnerStatusEntity = await _dbPartnerStatus.GetAsync(u => u.PartnerStatusId == id);
                if (partnerStatusEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PartnerStatusDTO>(partnerStatusEntity);
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
        public async Task<ActionResult<APIResponse>> CreatePartnerStatus([FromBody] PartnerStatusDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid partner status data.");
                }

                if (await _dbPartnerStatus.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "PartnerStatus already exists!");
                    return BadRequest(ModelState);
                }

                PartnerStatus partnerStatusEntity = _mapper.Map<PartnerStatus>(createDTO);
                await _dbPartnerStatus.CreateAsync(partnerStatusEntity);

                _response.Result = _mapper.Map<PartnerStatusDTO>(partnerStatusEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPartnerStatus", new { id = partnerStatusEntity.PartnerStatusId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePartnerStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePartnerStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var partnerStatusEntity = await _dbPartnerStatus.GetAsync(u => u.PartnerStatusId == id);
                if (partnerStatusEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPartnerStatus.DeletePartnerStatusAsync(partnerStatusEntity);

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
        [HttpPut("{id:int}", Name = "UpdatePartnerStatus")]
        public async Task<ActionResult<APIResponse>> UpdatePartnerStatus(int id, [FromBody] PartnerStatusDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.PartnerStatusId)
                {
                    return BadRequest();
                }

                PartnerStatus model = _mapper.Map<PartnerStatus>(updateDTO);

                await _dbPartnerStatus.UpdatePartnerStatusAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialPartnerStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialPartnerStatus(int id, JsonPatchDocument<PartnerStatusDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var partnerStatusEntity = await _dbPartnerStatus.GetAsync(u => u.PartnerStatusId == id, tracked: false);

            if (partnerStatusEntity == null)
            {
                return BadRequest();
            }

            PartnerStatusDTO partnerStatusDTO = _mapper.Map<PartnerStatusDTO>(partnerStatusEntity);
            patchDTO.ApplyTo(partnerStatusDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PartnerStatus model = _mapper.Map<PartnerStatus>(partnerStatusDTO);
            await _dbPartnerStatus.UpdatePartnerStatusAsync(model);

            return NoContent();
        }
    }
}
