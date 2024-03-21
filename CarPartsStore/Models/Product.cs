using System.ComponentModel.DataAnnotations;

namespace CarPartsStore.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string ImageUrl { get; set; } = "~/img/noimage.png";

        [Required(ErrorMessage = "*Please fill name field")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "*Please fill price field")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "*Please choose a category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
