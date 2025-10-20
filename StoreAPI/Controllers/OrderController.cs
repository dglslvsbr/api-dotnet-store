using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing orders
    /// </summary>
    [EnableRateLimiting("RateLimiter")]
    [EnableCors("AllowCors")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService orderService, ILogger<OrderController> logger) : ControllerBase
    {
        /// <summary>
        /// Get orders by page and size
        /// </summary>
        /// <response code="200">Orders retrieved successfully</response>
        /// <response code="404">No orders found</response>
        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("GetAllOrdersPaginated")]
        public async Task<ActionResult<IEnumerable<ShowOrderDTO>>> GetAllOrdersPaginatedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var orderList = await orderService.GetAllOrdersPaginatedAsync(pageNumber, pageSize);

            if (orderList is null || !orderList.Any())
                return NotFound(new Response<IActionResult>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = GlobalMessage.NotFound404
                });

            logger.LogInformation($"OrderController: Method GetAll was acioned successfully");
            return Ok(new Response<IEnumerable<ShowOrderDTO>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = GlobalMessage.OK200,
                Data = orderList
            });
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        /// <response code="200">Order found</response>
        /// <response code="404">Order not found</response>
        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var order = await orderService.GetAsync(id);

            if (order is null)
                return NotFound(new Response<IActionResult>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = GlobalMessage.NotFound404
                });

            logger.LogInformation($"OrderController: Method Get was acioned successfully");
            return Ok(new Response<ShowOrderDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = GlobalMessage.OK200,
                Data = order
            });
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <response code="201">Order created successfully</response>
        /// <response code="400">Invalid or null order data</response>
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderDTO createOrderDto)
        {
            var result = await orderService.CreateAsync(createOrderDto);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            logger.LogInformation($"OrderController: A new order was created successfully");
            return Ok(new Response<IActionResult>
            {
                StatusCode = StatusCodes.Status201Created,
                Message = GlobalMessage.Created201
            });
        }

        /// <summary>
        /// Atualizar a new order by ID
        /// </summary>
        /// <response code="200">Order updated successfully</response>
        /// <response code="400">Invalid order data or mismatched ID</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            if (updateOrderDto is null || updateOrderDto.Id != id)
                return BadRequest(new Response<IActionResult>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = GlobalMessage.BadRequest400
                });

            await orderService.UpdateAsync(updateOrderDto);

            logger.LogInformation($"OrderController: An order with ID {id} was updated successfully");
            return Ok(new Response<UpdateOrderDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = GlobalMessage.OK200,
                Data = updateOrderDto
            });
        }

        /// <summary>
        /// Delete a new order by ID
        /// </summary>
        /// <response code="200">Order deleted successfully</response>
        /// <response code="404">Order not found</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var orderExist = await orderService.GetAsync(id);

            if (orderExist is null)
                return NotFound(new Response<IActionResult>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = GlobalMessage.NotFound404
                });

            await orderService.DeleteAsync(orderExist);

            logger.LogInformation($"OrderController: An order with ID {id} was deleted successfully");
            return Ok(new Response<ShowOrderDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = GlobalMessage.OK200,
                Data = orderExist
            });
        }
    }
}