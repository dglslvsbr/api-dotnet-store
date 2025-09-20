using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers
{
    [EnableRateLimiting("RateLimiter")]
    [EnableCors("AllowCors")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();

            if (products is null || !products.Any())
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var produtsDto = _mapper.Map<IEnumerable<ShowProductDTO>>(products);

            _logger.LogInformation("ProductController: Method GetAll was acioned successfully");
            return Ok(new Response<IEnumerable<ShowProductDTO>> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = produtsDto });
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<ActionResult<Product>> GetAsync([FromRoute] int id)
        {
            var product = await _unitOfWork.ProductRepository.GetAsync(id);

            if (product is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var productDto = _mapper.Map<ShowProductDTO>(product);

            _logger.LogInformation("ProductController: Method Get was acioned successfully");
            return Ok(new Response<ShowProductDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = productDto });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductDTO createProductDto)
        {
            if (createProductDto is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var product = _mapper.Map<Product>(createProductDto);

            await _unitOfWork.ProductRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"ProductController: A new Product with ID {product.Id} was created successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status201Created, Message = GlobalMessage.Created201 });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch]
        [Route("Update/{id:int}")]
        public async Task<ActionResult<ShowProductDTO>> UpdateAsync([FromRoute] int id, [FromBody] JsonPatchDocument<Product> pathDoc)
        {
            if (pathDoc is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var productExist = await _unitOfWork.ProductRepository.GetAsync(id);

            if (productExist is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            pathDoc.ApplyTo(productExist, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            await _unitOfWork.CommitAsync();

            var productDto = _mapper.Map<ShowProductDTO>(productExist);

            _logger.LogInformation($"ProductController: A Product with ID {productDto.Id} was updated successfully");
            return Ok(new Response<ShowProductDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = productDto });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var productExist = await _unitOfWork.ProductRepository.GetAsync(id);

            if (productExist is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            _unitOfWork.ProductRepository.DeleteAsync(productExist);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"ProductController: A Product with ID {productExist.Id} was updated successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
        }
    }
}