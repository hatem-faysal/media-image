using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace image.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ?Name { get; set; }
        [NotMapped]
        public IFormFile ?Image { get; set; }        
    }
}