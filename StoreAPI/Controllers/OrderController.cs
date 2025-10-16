using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StoreAPI.DTOs;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Controllers
{
    [EnableRateLimiting("RateLimiter")]
    [EnableCors("AllowCors")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService orderRepository, ILogger<OrderController> logger) : ControllerBase
    {
        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("GetAllOrdersPaginated")]
        public async Task<ActionResult<IEnumerable<ShowOrderDTO>>> GetAllOrdersPaginatedAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var orderList = await orderRepository.GetAllOrdersPaginatedAsync(pageNumber, pageSize);

            if (orderList is null || !orderList.Any())
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            logger.LogInformation($"OrderController: Method GetAll was acioned successfully");
            return Ok(new Response<IEnumerable<ShowOrderDTO>> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = orderList });
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        [Route("Get/{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var order = await orderRepository.GetAsync(id);

            if (order is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            logger.LogInformation($"OrderController: Method Get was acioned successfully");
            return Ok(new Response<ShowOrderDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = order });
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderDTO createOrderDto)
        {
            if (createOrderDto is null)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            var result = await orderRepository.CreateAsync(createOrderDto);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            logger.LogInformation($"OrderController: A new order was created successfully");
            return Ok(new Response<IActionResult> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200 });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            if (updateOrderDto is null || updateOrderDto.Id != id)
                return BadRequest(new Response<IActionResult> { StatusCode = StatusCodes.Status400BadRequest, Message = GlobalMessage.BadRequest400 });

            await orderRepository.UpdateAsync(updateOrderDto);

            logger.LogInformation($"OrderController: An order with ID {id} was updated successfully");
            return Ok(new Response<UpdateOrderDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = updateOrderDto });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var orderExist = await orderRepository.GetAsync(id);

            if (orderExist is null)
                return NotFound(new Response<IActionResult> { StatusCode = StatusCodes.Status404NotFound, Message = GlobalMessage.NotFound404 });

            await orderRepository.DeleteAsync(orderExist);

            logger.LogInformation($"OrderController: An order with ID {id} was deleted successfully");
            return Ok(new Response<ShowOrderDTO> { StatusCode = StatusCodes.Status200OK, Message = GlobalMessage.OK200, Data = orderExist });
        }
    }
}