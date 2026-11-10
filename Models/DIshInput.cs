using System.ComponentModel.DataAnnotations;

namespace Top5.Models
{
    public class DishInput
    {
        [Required(ErrorMessage = "Give the dish a name")]
        [StringLength(40, ErrorMessage = "Keep the name to 40 characters")]
        public string Name { get; set; } = "";

        [Range(100, 10000, ErrorMessage = "Price must be between $100 and $10,000")]
        public int Price { get; set; }

        public bool IsSpicy { get; set; }

        public int? CategoryId { get; set; }
    }
}
