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
    public class MusicProviderOrderController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMusicProviderOrderRepository _dbMusicProviderOrder;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public MusicProviderOrderController(IMusicProviderOrderRepository dbMusicProviderOrder, IMapper mapper, ICacheService cacheService)
        {
            _dbMusicProviderOrder = dbMusicProviderOrder;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMusicProviderOrders()
        {
            try
            {
                var musicProviderOrderList = await _cacheService.GetOrAddAsync("musicProviderOrderListCache",
                   async () => await _dbMusicProviderOrder.GetAllAsync(),
                   TimeSpan.FromMinutes(1));
                //IEnumerable<MusicProviderOrder> musicProviderOrderList = await _dbMusicProviderOrder.GetAllAsync();
                _response.Result = _mapper.Map<List<MusicProviderOrderDTO>>(musicProviderOrderList);
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

        [HttpGet("{id:int}", Name = "GetMusicProviderOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMusicProviderOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var musicProviderOrderEntity = await _dbMusicProviderOrder.GetAsync(u => u.MusicProviderOrderId == id);
                if (musicProviderOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<MusicProviderOrderDTO>(musicProviderOrderEntity);
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
        public async Task<ActionResult<APIResponse>> CreateMusicProviderOrder([FromBody] MusicProviderOrderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Music Provider Order data.");
                }

                if (await _dbMusicProviderOrder.GetAsync(u => u.OrderName.ToLower() == createDTO.OrderName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorsMessages", "Music Provider Order already exists!");
                    return BadRequest(ModelState);
                }

                MusicProviderOrder musicProviderOrderEntity = _mapper.Map<MusicProviderOrder>(createDTO);
                await _dbMusicProviderOrder.CreateAsync(musicProviderOrderEntity);

                _response.Result = _mapper.Map<MusicProviderOrderDTO>(musicProviderOrderEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetMusicProviderOrder", new { id = musicProviderOrderEntity.MusicProviderOrderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteMusicProviderOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteMusicProviderOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var musicProviderOrderEntity = await _dbMusicProviderOrder.GetAsync(u => u.MusicProviderOrderId == id);
                if (musicProviderOrderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbMusicProviderOrder.DeleteMusicProviderOrderAsync(musicProviderOrderEntity);

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
        [HttpPut("{id:int}", Name = "UpdateMusicProviderOrder")]
        public async Task<ActionResult<APIResponse>> UpdateMusicProviderOrder(int id, [FromBody] MusicProviderOrderDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.MusicProviderOrderId)
                {
                    return BadRequest();
                }

                MusicProviderOrder model = _mapper.Map<MusicProviderOrder>(updateDTO);

                await _dbMusicProviderOrder.UpdateMusicProviderOrderAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialMusicProviderOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialMusicProviderOrder(int id, JsonPatchDocument<MusicProviderOrderDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var musicProviderOrderEntity = await _dbMusicProviderOrder.GetAsync(u => u.MusicProviderOrderId == id, tracked: false);

            if (musicProviderOrderEntity == null)
            {
                return BadRequest();
            }

            MusicProviderOrderDTO musicProviderOrderDTO = _mapper.Map<MusicProviderOrderDTO>(musicProviderOrderEntity);
            patchDTO.ApplyTo(musicProviderOrderDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MusicProviderOrder model = _mapper.Map<MusicProviderOrder>(musicProviderOrderDTO);
            await _dbMusicProviderOrder.UpdateMusicProviderOrderAsync(model);

            return NoContent();
        }
    }
}
