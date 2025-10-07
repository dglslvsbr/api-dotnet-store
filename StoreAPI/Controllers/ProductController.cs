using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers;

[EnableCors("AllowCors")]
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
{
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllProductsByCategory/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<ShowProductDTO>>> GetAllProductsByCategoryAsync([FromRoute] int categoryId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var productListPaginated = await productService.GetPaginatedProductsByCategoryAsync(categoryId, pageNumber, pageSize);

        if (productListPaginated is null || !productListPaginated.Any())
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        logger.LogInformation("ProductController: Method GetAll was acioned successfully");
        return Ok(new Response<IEnumerable<ShowProductDTO>> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = productListPaginated });
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllPaginated")]
    public async Task<ActionResult<IEnumerable<ShowProductDTO>>> GetAllPaginatedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var productListPaginated = await productService.GetAllPaginatedProductsAsync(pageNumber, pageSize);

        if (productListPaginated is null || !productListPaginated.Any())
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        logger.LogInformation("ProductController: Method GetAll was acioned successfully");
        return Ok(new Response<IEnumerable<ShowProductDTO>> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = productListPaginated });
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("Get/{id:int}")]
    public async Task<ActionResult<Product>> GetAsync([FromRoute] int id)
    {
        var product = await productService.GetAsync(id);

        if (product is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        logger.LogInformation("ProductController: Method Get was acioned successfully");
        return Ok(new Response<ShowProductDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = product });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductDTO createProductDto)
    {
        if (createProductDto is null)
            return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        await productService.CreateAsync(createProductDto);

        logger.LogInformation($"ProductController: A new Product with name {createProductDto.Name} was created successfully");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status201Created, Message = GlobalMessage.Created201 });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("Update/{id:int}")]
    public async Task<ActionResult<ShowProductDTO>> UpdateAsync([FromRoute] int id, [FromBody] UpdateProductDTO updateProductDTO)
    {
        if (updateProductDTO is null)
            return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

        var productExist = await productService.GetAsync(id);

        if (productExist is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        await productService.UpdateAsync(updateProductDTO);

        logger.LogInformation($"ProductController: A Product with ID {updateProductDTO.Id} was updated successfully");
        return Ok(new Response<ShowProductDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = productExist });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var productExist = await productService.GetAsync(id);

        if (productExist is null)
            return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

        await productService.DeleteAsync(productExist);

        logger.LogInformation($"ProductController: A Product with ID {productExist.Id} was updated successfully");
        return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
    }
}