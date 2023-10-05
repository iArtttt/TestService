using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.API.Dtos;
using Service.Common.Enums;
using Service.Common.Interfaces.Service;
using Service.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Vendor, Customer")]
    public class OrdersController : MainBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService, IMapper mapper, ILogger<OrdersController> logger)
            : base(mapper, logger)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> Get()
        {
            List<OrderModel>? orders;

            if (!User.IsInRole("Admin"))
            {
                var account = await GetCurrentAccount();
                if (account == null) return BadRequest();

                orders = User.IsInRole("Vendor")
                    ? await _orderService.GetAllByVendor(account.Id)
                    : await _orderService.GetAllByCustomer(account.Id);

            }
            else
            {
                orders = await _orderService.GetAll();
            }
            return Ok(_mapper.Map<OrderDto>(orders));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            OrderModel? order;

            if (!User.IsInRole("Admin"))
            {
                var account = await GetCurrentAccount();
                if (account == null) return BadRequest();

                order = User.IsInRole("Vendor")
                    ? await _orderService.GetByVendor(id, account.Id)
                    : await _orderService.GetByCustomer(id, account.Id);
            }
            else
                order = await _orderService.Get(id);

            if (order == null) return NotFound();

            return _mapper.Map<OrderDto>(order);
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<OrderStatusDto>> GetStatuses()
        {
            var statuses = _orderService.GetStatuses()
                .Select(s => new OrderStatusDto(s.name, s.id));
            return Ok(statuses);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto orderDto)
        {
            var customerId = orderDto.CustomerId;
            if (User.IsInRole("Admin"))
            {
                if (customerId == null) return BadRequest();
            }
            else
            {
                var account = (await GetCurrentAccount()) as CustomerAccountModel;
                if (account == null) return BadRequest();
                customerId = account.Id;
            }

            var order = await _orderService.Create(orderDto.DeliveryId, orderDto.Address, customerId.Value);
            if (order == null) return BadRequest();
            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpPost("{id:int}/product/{productId:int}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<OrderChangeDto>> AddProductToOrder(int id, int productId, [FromQuery][Range(1, int.MaxValue)] int? count)
        {
            count ??= 1;
            int? customerId = null;

            if (!User.IsInRole("Admin"))
            {
                var account = await GetCurrentAccount();
                if (account == null) return BadRequest();
                customerId = account.Id;
            }
            var result = await _orderService.AddProduct(id, productId, count.Value, customerId);
            if (!string.IsNullOrEmpty(result.messege))
                return BadRequest(new OrderChangeDto(false, productId, id, result.messege));

            return Ok(new OrderChangeDto(true, productId, id, null));
        }

        [HttpDelete("{id:int/product/{productId:int}}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<OrderChangeDto>> RemoveProductFromOrder(int id, int productId, [FromQuery][Range(1, int.MaxValue)] int? count, bool? all)
        {
            count ??= 1;
            all ??= false;
            int? customerId = null;

            if (!User.IsInRole("Admin"))
            {
                var account = await GetCurrentAccount();
                if (account == null) return BadRequest();
                customerId = account.Id;
            }

            var result = all.Value
                ? await _orderService.RemoveAllProduct(id, productId, customerId)
                : await _orderService.RemoveProduct(id, productId, count.Value, customerId);

            if (!string.IsNullOrEmpty(result.messege))
                return BadRequest(new OrderChangeDto(false, productId, id, result.messege));

            return Ok(new OrderChangeDto(true, productId, id, null));
        }

        [HttpPut("{id:int}/status/pending")]
        [Authorize(Roles = "Admin, Customer")]
        public Task<ActionResult<OrderDto>> ChangeStatusToPending(int id) => ChangeStatus(id, OrderStatus.Pending);

        [HttpPut("{id:int}/status/sent")]
        [Authorize(Roles = "Admin, Vendor")]
        public Task<ActionResult<OrderDto>> ChangeStatusToSent(int id) => ChangeStatus(id, OrderStatus.Sent);

        [HttpPut("{id:int}/status/done")]
        [Authorize(Roles = "Admin, Vendor")]
        public Task<ActionResult<OrderDto>> ChangeStatusToDone(int id) => ChangeStatus(id, OrderStatus.Done);

        [HttpPut("{id:int}/status/delete")]
        [Authorize(Roles = "Admin, Customer")]
        public Task<ActionResult<OrderDto>> DeleteOrder(int id) => ChangeStatus(id, OrderStatus.Deleted);

        private async Task<ActionResult<OrderDto>> ChangeStatus(int id, OrderStatus deleted)
        {
            int? customerId = null;
            int? vendorId = null;
            if (User.IsInRole("Customer"))
            {
                var account = await GetCurrentAccount();
                if (account == null) return BadRequest();
                customerId = account.Id;
            }
            if (User.IsInRole("Vendor"))
            {
                var account = await GetCurrentAccount();
                if (account == null) return BadRequest();
                vendorId = account.Id;
            }

            var result = await _orderService.ChangeStatus(id, deleted, customerId, vendorId);
            if (!string.IsNullOrEmpty(result.messege)) return BadRequest(result.messege);
            
            return Ok(_mapper.Map<OrderDto>(result.order));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, CreateOrderDto createOrder)
        {
            var customerId = createOrder.CustomerId;
            if (User.IsInRole("Admin"))
            {
                if (customerId == null) return BadRequest();
            }
            else
            {
                var account = (await GetCurrentAccount()) as CustomerAccountModel;
                if (account == null) return BadRequest();
                customerId = account.Id;
            }

            var order = await _orderService.Update(id, createOrder.DeliveryId, createOrder.Address, customerId.Value);
            if (order == null) return BadRequest();
            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpGet("{id:int}/changes")]
        public async Task<ActionResult<List<OrderLogDto>>> GetOrderLog(int id)
        {
            int? customerId = null;
            int? vendorId = null;
            if (User.IsInRole("Customer"))
            {
                var account = (await GetCurrentAccount()) as CustomerAccountModel;
                if (account == null) return BadRequest();
                customerId = account.Id;
            }
            else if (User.IsInRole("Vendor"))
            {
                var account = await GetCurrentAccount();
                if (account == null) return BadRequest();
                vendorId = account.Id;
            }
            var logs = await _orderService.GetChangeLogs(id, customerId, vendorId);
            return Ok(_mapper.Map<List<OrderLogDto>>(logs));
        }

    }
}
