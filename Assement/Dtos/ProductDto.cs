using Assement.Models;
using System.ComponentModel.DataAnnotations;

namespace Assement.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]

        public string Name { get; set; }
        [Required]

        public int Price { get; set; }

        [Required, StringLength(250)]
        public string Description { get; set; }

        [Display(Name ="Select Image...")]
        public byte[] poster { get; set; }

        [Display(Name = "Genre")]
        public byte GenreId { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
    }
}
