using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using sshBackend1.Models;
using sshBackend1.Models.DTOs;
using sshBackend1.Repository.IRepository;
using sshBackend1.Services.IServices;
using System.Net;

namespace sshBackend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicProviderController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMusicProviderRepository _dbMusicProvider;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public MusicProviderController(IMusicProviderRepository dbMusicProvider, IMapper mapper, ICacheService cacheService)
        {
            _dbMusicProvider = dbMusicProvider;
            _mapper = mapper;
            _response = new();
            _cacheService = cacheService;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMusicProviders()
        {
            try
            {
                var musicProviderList = await _cacheService.GetOrAddAsync("musicProviderListCache",
                   async () => await _dbMusicProvider.GetAllAsync(),
                   TimeSpan.FromMinutes(1));
                //IEnumerable<MusicProvider> musicProviderList = await _dbMusicProvider.GetAllAsync();
                _response.Result = _mapper.Map<List<MusicProviderDTO>>(musicProviderList);
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

        [HttpGet("{id:int}", Name = "GetMusicProvider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMusicProvider(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var musicProviderEntity = await _dbMusicProvider.GetAsync(u => u.MusicProviderId == id);
                if (musicProviderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<MusicProviderDTO>(musicProviderEntity);
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
        public async Task<ActionResult<APIResponse>> CreateMusicProvider([FromBody] MusicProviderDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("Invalid MusicP rovider data.");
                }

                if (await _dbMusicProvider.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Errors Messages", "Music Provider already exists!");
                    return BadRequest(ModelState);
                }

                MusicProvider musicProviderEntity = _mapper.Map<MusicProvider>(createDTO);
                await _dbMusicProvider.CreateAsync(musicProviderEntity);

                _response.Result = _mapper.Map<MusicProviderDTO>(musicProviderEntity);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetMusicProvider", new { id = musicProviderEntity.MusicProviderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteMusicProvider")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteMusicProvider(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var musicProviderEntity = await _dbMusicProvider.GetAsync(u => u.MusicProviderId == id);
                if (musicProviderEntity == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _dbMusicProvider.DeleteMusicProviderAsync(musicProviderEntity);

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
        [HttpPut("{id:int}", Name = "UpdateMusicProvider")]
        public async Task<ActionResult<APIResponse>> UpdateMusicProvider(int id, [FromBody] MusicProviderDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.MusicProviderId)
                {
                    return BadRequest();
                }

                MusicProvider model = _mapper.Map<MusicProvider>(updateDTO);

                await _dbMusicProvider.UpdateMusicProviderAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialMusicProvider")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialMusicProvider(int id, JsonPatchDocument<MusicProviderDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var musicProviderEntity = await _dbMusicProvider.GetAsync(u => u.MusicProviderId == id, tracked: false);

            if (musicProviderEntity == null)
            {
                return BadRequest();
            }

            MusicProviderDTO musicProviderDTO = _mapper.Map<MusicProviderDTO>(musicProviderEntity);
            patchDTO.ApplyTo(musicProviderDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MusicProvider model = _mapper.Map<MusicProvider>(musicProviderDTO);
            await _dbMusicProvider.UpdateMusicProviderAsync(model);

            return NoContent();
        }
    }
}
