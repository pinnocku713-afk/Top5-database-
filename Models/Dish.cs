using System.ComponentModel.DataAnnotations;

namespace Top5.Models
{
    // One dish on the menu. Properties use C# PascalCase: Name, Price, IsSpicy.
    // The [attributes] are the rules a dish has to pass before it goes on the menu.
    // [Display] sets the label text, and the name used in the built-in messages.
    public class Dish
    {
        public int Id { get; set; }

        [Display(Name = "Dish name")]
        [Required(ErrorMessage = "Give the dish a name")]
        [StringLength(40, ErrorMessage = "Keep the name to 40 characters")]
        public string Name { get; set; } = "";

        [Display(Name = "Price (JMD)")]
        [Range(100, 10000, ErrorMessage = "Price must be between $100 and $10,000")]
        public int Price { get; set; }

        [Display(Name = "Spicy?")]
        public bool IsSpicy { get; set; }
        
        [Display(Name = "Vegetarian?")]
        public bool IsVegetarian { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
