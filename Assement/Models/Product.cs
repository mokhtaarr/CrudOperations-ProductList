using System.ComponentModel.DataAnnotations;

namespace Assement.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }

        public int Price { get; set; }

        [Required, MaxLength(250)]
        public string Description { get; set; }

        public byte[] poster { get; set; }

        [Display(Name ="Select Category")]
        public byte GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
