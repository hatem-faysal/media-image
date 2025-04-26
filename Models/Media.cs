using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace image.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }
        public string Model { get; set; }
        [Column("collection_name")]
        public string CollectionName { get; set; }
        public string Name { get; set; }
        [Column("id_related")]
        public int IdRelated { get; set; }
        [Column("file_size")]
        public string FileSize { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}