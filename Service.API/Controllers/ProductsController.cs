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
    public class ProductsController : MainBaseController
    {
        private readonly IProductRepository _product;

        public ProductsController(IProductRepository product, IMapper mapper, ILogger<ProductsController> logger) : base(mapper, logger)
        {
            _product = product;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> Get([FromQuery] int? vendor, [FromQuery] string? category)
        {
            string? vendorLogin = null;
            if (User.IsInRole("Vendor") && !User.IsInRole("Admin"))
            {
                var login = GetCurrentAccountLogin();
                if (login != null) BadRequest();
                vendorLogin = login;
                vendor = null;
            }

            var products = await _product.GetAll(vendor, vendorLogin, category);
            return Ok(_mapper.Map<List<ProductDto>>(products));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            string? vendor = null;
            if (User.IsInRole("Vendor") && !User.IsInRole("Admin"))
            {
                var login = GetCurrentAccountLogin();
                if (login != null) BadRequest();
                vendor = login;
            }

            var products = await _product.Get(id, vendor);
            if (products == null) return NotFound();

            return Ok(_mapper.Map<ProductDto>(products));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Vendor")]
        public async Task<ActionResult<ProductDto>> Create(CreateProductDto productDto)
        {
            var product = _mapper.Map<ProductModel>(productDto);

            if (User.IsInRole("Vendor") && !User.IsInRole("Admin"))
            {
                var user = await GetCurrentAccount();
                if (user == null) return BadRequest();
                product.User = user;
            }
            product = await _product.Create(product);
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin, Vendor")]
        public async Task<ActionResult<ProductDto>> Update(int id, CreateProductDto productDto)
        {
            int? vendorId = null;

            if (User.IsInRole("Vendor") && !User.IsInRole("Admin"))
            {
                var user = await GetCurrentAccount();
                if (user == null) return BadRequest();
                vendorId = user.Id;
                productDto = productDto with { VendorId = vendorId.Value };
            }
            if (!await _product.IsProductExist(id, vendorId)) return NotFound();

            var product = _mapper.Map<ProductModel>(productDto);
            product.Id = id;
            product = await _product.Update(product);
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin, Vendor")]
        public async Task<ActionResult> Delete(int id)
        {
            int? vendorId = null;

            if (User.IsInRole("Vendor") && !User.IsInRole("Admin"))
            {
                var user = await GetCurrentAccount();
                if (user == null) return BadRequest();
                vendorId = user.Id;
            }
            if (!await _product.IsProductExist(id, vendorId)) return NotFound();

            await _product.Remove(id);

            return Ok();
        }
    }
}
