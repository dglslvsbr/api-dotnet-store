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
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        private readonly ISystemCache<Category> _systemCache;
        private const string CacheCategory = "CacheCategory";

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryController> logger, ISystemCache<Category> systemCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _systemCache = systemCache;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ShowCategoryDTO>>> GetAllAsync()
        {
            var categories = await _systemCache.TryGetCacheList(CacheCategory);

            if (categories is null || !categories.Any())
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var categorysDto = _mapper.Map<IEnumerable<ShowCategoryDTO>>(categories);

            _logger.LogInformation("CategoryController: Method GetAll was acioned successfully");
            return Ok(new Response<IEnumerable<ShowCategoryDTO>> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = categorysDto });
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<ActionResult<ShowCategoryDTO>> GetAsync([FromRoute] int id)
        {
            var category = await _systemCache.TryGetCacheUnique($"{CacheCategory}/{id}", id);

            if (category is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var categoryDto = _mapper.Map<ShowCategoryDTO>(category);

            _logger.LogInformation("CategoryController: Method Get was acioned successfully");
            return Ok(new Response<ShowCategoryDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = categoryDto });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryDTO createCategoryDto)
        {
            if (createCategoryDto is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var category = _mapper.Map<Category>(createCategoryDto);

            await _unitOfWork.CategoryRepository.CreateAsync(category);
            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheCategory);

            _logger.LogInformation($"CategoryController: A Category with ID {category.Id} was created successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status201Created, Message = GlobalMessage.Created201 });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch]
        [Route("Update/{id:int}")]
        public async Task<ActionResult<ShowCategoryDTO>> UpdateAsync([FromRoute] int id, [FromBody] JsonPatchDocument<Category> pathDoc)
        {
            if (pathDoc is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var categoryExist = await _unitOfWork.CategoryRepository.GetAsync(id);

            if (categoryExist is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            pathDoc.ApplyTo(categoryExist, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheCategory);
            _systemCache.InvalidCache($"{CacheCategory}/{id}");

            var categoryDto = _mapper.Map<ShowCategoryDTO>(categoryExist);

            _logger.LogInformation($"CategoryController: A Category with ID {categoryDto.Id} was updated successfully");
            return Ok(new Response<ShowCategoryDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = categoryDto });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var categoryExist = await _unitOfWork.CategoryRepository.GetAsync(id);

            if (categoryExist is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            _unitOfWork.CategoryRepository.DeleteAsync(categoryExist);
            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheCategory);
            _systemCache.InvalidCache($"{CacheCategory}/{id}");

            _logger.LogInformation($"CategoryController: A Category with ID {id} was deleted successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
        }
    }
}