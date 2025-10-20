using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers;

/// <summary>
/// Controller responsible for managing products
/// </summary>
[EnableCors("AllowCors")]
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
{
    /// <summary>
    /// Get paginated products by category
    /// </summary>
    /// <response code="200">Products retrieved successfully</response>
    /// <response code="404">No products found for this category</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllProductsByCategory/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<ShowProductDTO>>> GetAllProductsByCategoryAsync([FromRoute] int categoryId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var productListPaginated = await productService.GetPaginatedProductsByCategoryAsync(categoryId, pageNumber, pageSize);

        if (productListPaginated is null || !productListPaginated.Any())
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        logger.LogInformation("ProductController: Method GetAll was acioned successfully");
        return Ok(new Response<IEnumerable<ShowProductDTO>>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = productListPaginated
        });
    }

    /// <summary>
    /// Get products by page and size
    /// </summary>
    /// <response code="200">Products retrieved successfully</response>
    /// <response code="404">No products found</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllPaginated")]
    public async Task<ActionResult<IEnumerable<ShowProductDTO>>> GetAllPaginatedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var productListPaginated = await productService.GetAllPaginatedProductsAsync(pageNumber, pageSize);

        if (productListPaginated is null || !productListPaginated.Any())
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        logger.LogInformation("ProductController: Method GetAll was acioned successfully");
        return Ok(new Response<IEnumerable<ShowProductDTO>>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = productListPaginated
        });
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    /// <response code="200">Product found</response>
    /// <response code="404">Product not found</response>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("Get/{id:int}")]
    public async Task<ActionResult<Product>> GetAsync([FromRoute] int id)
    {
        var product = await productService.GetAsync(id);

        if (product is null)
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        logger.LogInformation("ProductController: Method Get was acioned successfully");
        return Ok(new Response<ShowProductDTO>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = product
        });
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <response code="201">Product created successfully</response>
    /// <response code="400">Invalid or null product data</response>
   
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductDTO createProductDto)
    {
        if (createProductDto is null)
            return BadRequest(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = GlobalMessage.BadRequest400
            });

        await productService.CreateAsync(createProductDto);

        logger.LogInformation($"ProductController: A new Product with name {createProductDto.Name} was created successfully");
        return Ok(new Response<IActionResult>
        {
            StatusCode = StatusCodes.Status201Created,
            Message = GlobalMessage.Created201
        });
    }

    /// <summary>
    /// Update a new product by ID
    /// </summary>
    /// <response code="200">Product updated successfully</response>
    /// <response code="400">Invalid product data</response>
    /// <response code="404">Product not found</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("Update/{id:int}")]
    public async Task<ActionResult<ShowProductDTO>> UpdateAsync([FromRoute] int id, [FromBody] UpdateProductDTO updateProductDTO)
    {
        if (updateProductDTO is null)
            return BadRequest(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = GlobalMessage.BadRequest400
            });

        var productExist = await productService.GetAsync(id);

        if (productExist is null)
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        await productService.UpdateAsync(updateProductDTO);

        logger.LogInformation($"ProductController: A Product with ID {updateProductDTO.Id} was updated successfully");
        return Ok(new Response<ShowProductDTO>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200,
            Data = productExist
        });
    }

    /// <summary>
    /// Delete a new product by ID
    /// </summary>
    /// <response code="200">Product deleted successfully</response>
    /// <response code="404">Product not found</response>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var productExist = await productService.GetAsync(id);

        if (productExist is null)
            return NotFound(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = GlobalMessage.NotFound404
            });

        await productService.DeleteAsync(productExist);

        logger.LogInformation($"ProductController: A Product with ID {productExist.Id} was updated successfully");
        return Ok(new Response<IActionResult>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = GlobalMessage.OK200
        });
    }
}