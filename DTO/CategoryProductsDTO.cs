using API_1.Models;
using System.Text.Json.Serialization;

namespace API_1.DTO
{
    public class CategoryProductsDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string[] productsNames { get; set; }
    }
}
