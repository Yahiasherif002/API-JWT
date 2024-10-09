using API_1.DTO;
using API_1.Models;
using API_1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly AppDbContext _context;

        public ProductController(IGenericRepository<Product> repository , AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (product == null)
                return BadRequest("Product cannot be null.");

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("GetProductById", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (product == null)
                return BadRequest("Product cannot be null.");

            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("FULL")]
        public async Task<IActionResult> AddProduct(ProductDTO productDTO)
        {
            var category = await _context.categories.FindAsync(productDTO.CategoryId);

            if(category is null)
            {
                return BadRequest("Category does not exist.");
            }

            var product = new Product
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                CategoryId = productDTO.CategoryId
            };

            await _repository.AddAsync(product);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
