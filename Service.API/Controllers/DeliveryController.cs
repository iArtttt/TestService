using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.API.Dtos;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;

namespace Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DeliveryController : MainBaseController
    {
        private readonly IDeliveryRepository _repository;

        public DeliveryController(IMapper mapper, ILogger<DeliveryController> logger, IDeliveryRepository repository) : base(mapper, logger)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer, Vendor")]
        public async Task<ActionResult<List<DeliveryDto>>> GetAll()
        {
            var result = await _repository.Get();
            return Ok(_mapper.Map<List<DeliveryDto>>(result));
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, Customer, Vendor")]
        public async Task<ActionResult<DeliveryDto>> Get(int id)
        {
            var result = await _repository.Get(id);
            if (result == null) NotFound();
            return Ok(_mapper.Map<DeliveryDto>(result));
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryDto>> Create(CreateDeliveryDto deliveryDto)
        {
            var delivery = await _repository.Create(deliveryDto.Name);
            return Ok(_mapper.Map<DeliveryDto>(delivery));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<DeliveryDto>> Update(int id, CreateDeliveryDto deliveryDto)
        {
            var model = _mapper.Map<DeliveryModel>(deliveryDto);
            model.Id = id;

            var result = await _repository.Update(model);
            if (!result) return NotFound();

            return Ok(new DeliveryDto(id, deliveryDto.Name));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<DeliveryDto>> Delete(int id)
        {
            var result = await _repository.Remove(id);
            if (!result) return NotFound();

            return Ok();
        }
    }
}
