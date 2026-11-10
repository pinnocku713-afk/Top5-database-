namespace Top5.Models
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Price { get; set; }
        public bool IsSpicy { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
