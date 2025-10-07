using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Entities.Authentication;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers;

[EnableRateLimiting("RateLimiter")]
[EnableCors("AllowCors")]
[ApiController]
[Route("api/[controller]")]
public class AuthController(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService, IMapper mapper, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDTO)
    {
        if (loginDTO is null)
            return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        var userExist = await unitOfWork.ClientRepository.GetByEmailAsync(loginDTO.Email!);

        if (userExist is null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, userExist.Password))
            return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        var token = tokenService.GenerateToken(userExist, configuration);

        logger.LogInformation($"AuthController: The Client with ID {userExist.Id} has connected");
        return Ok(new Response<string> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = token });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleDTO createRoleDto)
    {
        if (createRoleDto is null)
            return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        var roleExist = await unitOfWork.RoleRepository.GetByNameAsync(createRoleDto.Name!);

        if (roleExist is not null)
            return Conflict(new Response<IActionResult> { StatusCode = StatusCodes.Status409Conflict, Message = GlobalMessage.Conflit409 });

        var role = mapper.Map<Role>(createRoleDto);
        await unitOfWork.RoleRepository.CreateAsync(role);
        await unitOfWork.CommitAsync();

        logger.LogInformation($"AuthController: A Role with ID {role.Id} was created successfully");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status201Created, Message = GlobalMessage.Created201 });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("AddRole/{email}/{roleName}")]
    public async Task<IActionResult> AddRoleAsync([FromRoute] string email, [FromRoute] string roleName)
    {
        var clientExist = await unitOfWork.ClientRepository.GetByEmailAsync(email);
        var roleExist = await unitOfWork.RoleRepository.GetByNameAsync(roleName);

        if (clientExist is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        if (roleExist is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        foreach (var obj in clientExist.ClientRole!)
            if (obj.Role!.Name == roleName)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        clientExist.ClientRole.Add(new ClientRole { RoleId = roleExist.Id, ClientId = clientExist.Id });

        await unitOfWork.CommitAsync();

        logger.LogInformation($"AuthController: The Role {roleExist.Name} was assigned to Client with ID: {clientExist.Id}");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("DeleteRole/{id:int}")]
    public async Task<IActionResult> DeleteRoleAsync([FromRoute] int id)
    {
        var roleExist = await unitOfWork.RoleRepository.GetAsync(id);

        if (roleExist is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        unitOfWork.RoleRepository.Delete(roleExist);

        logger.LogInformation($"The Role with ID: {roleExist.Id} was deleted successfully");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
    }
}