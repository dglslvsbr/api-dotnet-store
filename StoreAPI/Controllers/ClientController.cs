using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Entities.Authentication;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers
{
    [EnableRateLimiting("RateLimiter")]
    [EnableCors("AllowCors")]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientController> _logger;
        private readonly ISystemCache<Client> _systemCache;
        private const string CacheClient = "CacheClient";

        public ClientController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClientController> logger, ISystemCache<Client> systemCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _systemCache = systemCache;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ShowClientDTO>>> GetAllAsync()
        {
            var clients = await _systemCache.TryGetCacheList(CacheClient);

            if (clients is null || !clients.Any())
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var clientsDto = _mapper.Map<ShowClientDTO>(clients);
            _logger.LogInformation("ClientController: Method GetAll was acioned successfully");
            return Ok(new Response<ShowClientDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = clientsDto });

        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<ActionResult<ShowClientDTO>> GetAsync([FromRoute] int id)
        {
            var client = await _systemCache.TryGetCacheUnique($"{CacheClient}/{id}", id);

            if (client is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var clientDto = _mapper.Map<ShowClientDTO>(client);
            _logger.LogInformation("ClientController: Method Get was acioned successfully");
            return Ok(new Response<ShowClientDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = clientDto });
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateClientDTO createClientDto)
        {
            if (createClientDto is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var clientExist = await _unitOfWork.ClientRepository.GetByEmailAsync(createClientDto.Email!);

            if (clientExist is not null)
                return Conflict(new Response<IActionResult> { StatusCode = StatusCodes.Status409Conflict, Message = GlobalMessage.Conflit409 });

            var client = _mapper.Map<Client>(createClientDto);

            client.Password = BCrypt.Net.BCrypt.HashPassword(client.Password);
            client.ClientRole = [new ClientRole { RoleId = 1, ClientId = client.Id }];

            await _unitOfWork.ClientRepository.CreateAsync(client);
            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheClient);

            _logger.LogInformation($"ClientController: A Client with ID {client.Id} was created successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status201Created, Message = GlobalMessage.OK200 });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch]
        [Route("Update/{id:int}")]
        public async Task<ActionResult<ShowClientDTO>> UpdateAsync([FromRoute] int id, [FromBody] JsonPatchDocument<Client> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var client = await _unitOfWork.ClientRepository.GetAsync(id);

            if (client is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            patchDoc.ApplyTo(client, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var clientDto = _mapper.Map<ShowClientDTO>(client);

            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheClient);
            _systemCache.InvalidCache($"{CacheClient}/{id}");

            _logger.LogInformation($"ClientController: A Client with ID {client.Id} was updated successfully");
            return Ok(new Response<ShowClientDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = clientDto });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var clientExist = await _unitOfWork.ClientRepository.GetAsync(id);

            if (clientExist is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            _unitOfWork.ClientRepository.DeleteAsync(clientExist);

            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheClient);
            _systemCache.InvalidCache($"{CacheClient}/{id}");

            _logger.LogInformation($"ClientController: A Client with ID {clientExist.Id} was deleted successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
        }
    }
}