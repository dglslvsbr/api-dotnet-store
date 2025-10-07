using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers;

[EnableRateLimiting("RateLimiter")]
[EnableCors("AllowCors")]
[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : ControllerBase
{
    [Authorize("UserOnly")]
    [HttpGet]
    [Route("GetAllCategoriesPaginated")]
    public async Task<ActionResult<IEnumerable<ShowCategoryDTO>>> GetAllCategoriesPaginatedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var categoriesList = await categoryService.GetAllCategoriesPaginatedAsync(pageNumber, pageSize);

        if (categoriesList is null || !categoriesList.Any())
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        logger.LogInformation("Method Get All Categories Paginated was acioned successfully");
        return Ok(new Response<IEnumerable<ShowCategoryDTO>> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = categoriesList });
    }

    [Authorize("UserOnly")]
    [HttpGet]
    [Route("GetCategoryWithProducts/{id:int}")]
    public async Task<ActionResult<ShowCategoryDTO>> GetCategoryWithProductsAsync([FromRoute] int id)
    {
        var category = await categoryService.GetCategoryWithProductsAsync(id);

        if (category is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        logger.LogInformation("Method Get was acioned successfully");
        return Ok(new Response<ShowCategoryDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = category });
    }

    [Authorize("UserOnly")]
    [HttpGet]
    [Route("Get/{id:int}")]
    public async Task<ActionResult<ShowCategoryDTO>> GetAsync([FromRoute] int id)
    {
        var category = await categoryService.GetAsync(id);

        if (category is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        logger.LogInformation("Method Get was acioned successfully");
        return Ok(new Response<ShowCategoryDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = category });
    }

    [Authorize("AdminOnly")]
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryDTO categoryDto)
    {
        if (categoryDto is null)
            return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        await categoryService.CreateAsync(categoryDto);

        logger.LogInformation($"A new category with name {categoryDto.Name} was created successfully");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
    }

    [Authorize("AdminOnly")]
    [HttpPut]
    [Route("Update/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateCategoryDTO updateCategoryDto)
    {
        if (updateCategoryDto is null || updateCategoryDto.Id != id)
            return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        await categoryService.UpdateAsync(updateCategoryDto);

        logger.LogInformation($"A category with ID {id} was updated successfully");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
    }

    [Authorize("AdminOnly")]
    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var categoryExist = await categoryService.GetAsync(id);

        if (categoryExist is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        await categoryService.DeleteAsync(categoryExist);

        logger.LogInformation($"A category with ID {id} was deleted successfully");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
    }
}