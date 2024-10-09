using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_1.Models
{
    public class Product
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
       
        public string? Description { get; set; }

        

        public int? CategoryId { get; set; }
       

        public Category category { get; set; }

    }
}
