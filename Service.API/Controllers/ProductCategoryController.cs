using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.API.Dtos;
using Service.Common.Interfaces.Repositories;
using Service.Common.Models;
using Service.Domain.Models;

namespace Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : MainBaseController
    {
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductCategoryController(IProductCategoryRepository categoryRepository,IMapper mapper, ILogger<ProductCategoryController> logger) : base(mapper, logger)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll()
        {
            var categories = await _categoryRepository.Get();
            return Ok(_mapper.Map<List<CategoryDto>>(categories));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var category = await _categoryRepository.Get(id);
            if (category == null) return NotFound();

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto createCategory)
        {
            var newCategory = _mapper.Map<ProductCategoryModel>(createCategory);
            newCategory = await _categoryRepository.Create(newCategory);

            return Ok(_mapper.Map<CategoryDto>(newCategory));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> Update(int id, CreateCategoryDto createCategory)
        {
            var toUpdateCategory = _mapper.Map<ProductCategoryModel>(createCategory);
            toUpdateCategory.Id = id;
            
            var result = await _categoryRepository.Update(toUpdateCategory);
            if (!result) return NotFound("Category not found.");

            return Ok(_mapper.Map<CategoryDto>(toUpdateCategory));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _categoryRepository.Remove(id);
            if (!result) return NotFound();

            return Ok();
        }
    }
}
