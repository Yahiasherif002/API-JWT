using API_1.DTO;
using API_1.Models;
using API_1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> genericRepository;
        private readonly AppDbContext context;

        public CategoryController(IGenericRepository<Category> genericRepository , AppDbContext context) 
        
        {
            this.genericRepository = genericRepository;
            this.context = context;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<CategoryProductsDTO>> GetCategorywithProductAsync(int id)
        {
            var category = await context.categories
                .Include(c => c.products) 
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound(); // Return 404 if not found
            }
            else
            {

                CategoryProductsDTO categoryProductsDTO = new CategoryProductsDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    productsNames = category.products.Select(p => p.Name).ToArray()
                };
                return Ok(categoryProductsDTO);
            }
            return Unauthorized("Unauthorized access. Please check your credentials.");
        }

        [HttpPost]
        public async Task <IActionResult> Add(Category category)
        {
            await genericRepository.AddAsync(category);

            await genericRepository.SaveChangesAsync();


            return NoContent();
        }


            
    }
}
