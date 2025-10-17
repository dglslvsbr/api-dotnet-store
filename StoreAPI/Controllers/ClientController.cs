using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Enums;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers;

/// <summary>
/// Controller responsible for managing clients
/// </summary>
[EnableRateLimiting("RateLimiter")]
[EnableCors("AllowCors")]
[ApiController]
[Route("api/[controller]")]
public class ClientController(IClientService clientService, ILogger<ClientController> logger) : ControllerBase
{
    /// <summary>
    /// Get all clients
    /// </summary>
    /// <response code="200">Clients retrieved successfully</response>
    /// <response code="404">No clients found</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<IEnumerable<ShowClientDTO>>> GetAllAsync()
    {
        var clientList = await clientService.GetAllAsync();

        if (clientList is null || !clientList.Any())
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        logger.LogInformation("ClientController: Method GetAll was acioned successfully");
        return Ok(new Response<IEnumerable<ShowClientDTO>>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = clientList
        });
    }

    /// <summary>
    /// Get client by ID
    /// </summary>
    /// <response code="200">Client found</response>
    /// <response code="404">Client not found</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("Get/{id:int}")]
    public async Task<ActionResult<ShowClientDTO>> GetAsync([FromRoute] int id)
    {
        var client = await clientService.GetAsync(id);

        if (client is null)
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        logger.LogInformation("ClientController: Method Get was acioned successfully");
        return Ok(new Response<ShowClientDTO>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = client
        });
    }

    /// <summary>
    /// Get client By Email
    /// </summary>
    /// <response code="200">Client found</response>
    /// <response code="404">Client not found</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetByEmail/{email}")]
    public async Task<ActionResult<ShowClientDTO>> GetAsync([FromRoute] string email)
    {
        var client = await clientService.GetByEmailAsync(email);

        if (client is null)
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        logger.LogInformation("ClientController: Method Get was acioned successfully");
        return Ok(new Response<ShowClientDTO>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = client
        });
    }

    /// <summary>
    /// Create a new client
    /// </summary>
    /// <response code="201">Client created successfully</response>
    /// <response code="400">Invalid or null client data</response>
    /// <response code="409">Email, CPF, or phone already exist</response>
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateAccountAsync([FromBody] CreateClientDTO createClientDto)
    {
        if (createClientDto is null)
            return BadRequest(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = GlobalMessage.BadRequest400
            });

        var clientDuplicate = await clientService.CheckDuplicates(createClientDto);

        if (clientDuplicate.Contains(DuplicateField.Email_Exist))
            return Conflict(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status409Conflict,
                Message = "Email already exist"
            });

        if (clientDuplicate.Contains(DuplicateField.CPF_Exist))
            return Conflict(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status409Conflict,
                Message = "CPF already exist"
            });

        if (clientDuplicate.Contains(DuplicateField.Phone_Exist))
            return Conflict(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status409Conflict,
                Message = "Phone already exist"
            });

        await clientService.CreateAsync(createClientDto);

        logger.LogInformation($"ClientController: A Client with Email {createClientDto.Email} was created successfully");
        return Ok(new Response<IActionResult>
        {
            StatusCode = StatusCodes.Status201Created,
            Message = GlobalMessage.Created201
        });
    }

    /// <summary>
    /// Update a client by ID
    /// </summary>
    /// <response code="200">Client updated successfully</response>
    /// <response code="400">Invalid client data or mismatched ID</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("Update/{id:int}")]
    public async Task<ActionResult<ShowClientDTO>> UpdateAsync([FromRoute] int id, [FromBody] UpdateClientDTO updateClientDTO)
    {
        if (updateClientDTO is null || updateClientDTO.Id != id)
            return BadRequest(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = GlobalMessage.BadRequest400
            });

        await clientService.UpdateAsync(updateClientDTO);

        logger.LogInformation($"ClientController: A Client with ID {updateClientDTO.Id} was updated successfully");
        return Ok(new Response<UpdateClientDTO>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = updateClientDTO
        });
    }

    /// <summary>
    /// Delete a client by ID
    /// </summary>
    /// <response code="200">Client deleted successfully</response>
    /// <response code="404">Client not found</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var clientExist = await clientService.GetAsync(id);

        if (clientExist is null)
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        await clientService.DeleteAsync(clientExist);

        logger.LogInformation($"ClientController: A Client with ID {id} was deleted successfully");
        return Ok(new Response<IActionResult>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200
        });
    }
}