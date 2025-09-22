using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Entities.Models;
using StoreAPI.Enums;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers
{
    [EnableRateLimiting("RateLimiter")]
    [EnableCors("AllowCors")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        private readonly ISystemCache<Order> _systemCache;
        private const string CacheOrder = "CacheOrder";

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderController> logger, ISystemCache<Order> systemCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _systemCache = systemCache;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ShowOrderDTO>>> GetAllAsync()
        {
            var orders = await _systemCache.TryGetCacheList(CacheOrder);

            if (orders is null || !orders.Any())
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var ordersDto = _mapper.Map<IEnumerable<ShowOrderDTO>>(orders);

            _logger.LogInformation($"OrderController: Method GetAll was acioned successfully");
            return Ok(new Response<IEnumerable<ShowOrderDTO>> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = ordersDto });
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var order = await _systemCache.TryGetCacheUnique($"{CacheOrder}/{id}", id);

            if (order is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            var orderDto = _mapper.Map<ShowOrderDTO>(order);

            _logger.LogInformation($"OrderController: Method Get was acioned successfully");
            return Ok(new Response<ShowOrderDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = orderDto });
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost]
        [Route("Create/{clientId:int}/{installments:int}")]
        public async Task<IActionResult> CreateAsync([FromRoute] int clientId, [FromRoute] int installments, [FromBody] List<Product> products)
        {
            try
            {
                if (products is null || products.Count == 0)
                    return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

                foreach (Product p in products)
                    if (await _unitOfWork.ProductRepository.GetAsync(p.Id) is null)
                        return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

                var orderItems = products.Select(p => new OrderItem
                {
                    ProductId = p.Id,
                    UnitPrice = p.Price,
                    Quantity = p.Quantity
                }).ToList();

                var order = new Order
                {
                    CreatAt = DateTimeOffset.UtcNow,
                    CurrentState = OrderState.Processing,
                    Installments = installments,
                    ClientId = clientId,
                    OrderItem = orderItems
                };

                await _unitOfWork.OrderRepository.CreateAsync(order);
                await _unitOfWork.CommitAsync();

                _systemCache.InvalidCache(CacheOrder);

                _logger.LogInformation($"OrderController: A new Order with ID {order.Id} was created successfully");
                return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status201Created, Message = GlobalMessage.Created201 });
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] JsonPatchDocument<Order> pathDoc)
        {
            if (pathDoc is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var order = await _unitOfWork.OrderRepository.GetAsync(id);

            if (order is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            pathDoc.ApplyTo(order, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheOrder);
            _systemCache.InvalidCache($"{CacheOrder}/{id}");

            var orderDto = _mapper.Map<ShowOrderDTO>(order);

            _logger.LogInformation($"OrderController: An Order with ID {orderDto.Id} was updated successfully");
            return Ok(new Response<ShowOrderDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = orderDto });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var orderExist = await _unitOfWork.OrderRepository.GetAsync(id);

            if (orderExist is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            _unitOfWork.OrderRepository.DeleteAsync(orderExist);
            await _unitOfWork.CommitAsync();

            _systemCache.InvalidCache(CacheOrder);
            _systemCache.InvalidCache($"{CacheOrder}/{id}");

            _logger.LogInformation($"OrderController: An Order with ID {orderExist.Id} was deleted successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
        }
    }
}