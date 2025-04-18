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
    public class PlaylistItemController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPlaylistItemRepository _dbPlaylistItem;
        private readonly IMapper _mapper;

        public PlaylistItemController(IPlaylistItemRepository dbPlaylistItem, IMapper mapper)
        {
            _dbPlaylistItem = dbPlaylistItem;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPlaylistItems()
        {
            try
            {
                IEnumerable<PlaylistItem> PlaylistItemList = await _dbPlaylistItem.GetAllAsync();
                _response.Result = _mapper.Map<List<PlaylistItemDTO>>(PlaylistItemList);
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

        [HttpGet("{id:int}", Name = "GetPlaylistItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPlaylistItem(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var playlistItemEntity = await _dbPlaylistItem.GetAsync(u => u.PlaylistItemId == id);
                if (playlistItemEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PlaylistItemDTO>(playlistItemEntity);
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
        public async Task<ActionResult<APIResponse>> CreatePlaylistItem([FromBody] PlaylistItemDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid Playlist Item data.");
                }

                if (await _dbPlaylistItem.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Errors Messages", "Playlist Item already exists!");
                    return BadRequest(ModelState);
                }

                PlaylistItem playlistItemEntity = _mapper.Map<PlaylistItem>(createDTO);
                await _dbPlaylistItem.CreateAsync(playlistItemEntity);

                _response.Result = _mapper.Map<PlaylistItemDTO>(playlistItemEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPlaylistItem", new { id = playlistItemEntity.PlaylistItemId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePlaylistItem")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePlaylistItem(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var playlistItemEntity = await _dbPlaylistItem.GetAsync(u => u.PlaylistItemId == id);
                if (playlistItemEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbPlaylistItem.DeletePlaylistItemAsync(playlistItemEntity);

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
        [HttpPut("{id:int}", Name = "UpdatePlaylistItem")]
        public async Task<ActionResult<APIResponse>> UpdatePlaylistItem(int id, [FromBody] PlaylistItemDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.PlaylistItemId)
                {
                    return BadRequest();
                }

                PlaylistItem model = _mapper.Map<PlaylistItem>(updateDTO);

                await _dbPlaylistItem.UpdatePlaylistItemAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialPlaylistItem")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialPlaylistItem(int id, JsonPatchDocument<PlaylistItemDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var playlistItemEntity = await _dbPlaylistItem.GetAsync(u => u.PlaylistItemId == id, tracked: false);

            if (playlistItemEntity == null)
            {
                return BadRequest();
            }

            PlaylistItemDTO playlistItemDTO = _mapper.Map<PlaylistItemDTO>(playlistItemEntity);
            patchDTO.ApplyTo(playlistItemDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PlaylistItem model = _mapper.Map<PlaylistItem>(playlistItemDTO);
            await _dbPlaylistItem.UpdatePlaylistItemAsync(model);

            return NoContent();
        }


    }
}
