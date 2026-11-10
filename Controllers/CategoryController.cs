using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Top5.Data;
using Top5.Models;

namespace Top5.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db) { _db = db;}
        [HttpGet("categories/{id:int}")]
        public IActionResult Details(int id)
        {
            Category? category = _db.Categories
                .Include(each_category => each_category.Dishes)
                .FirstOrDefault(each_category => each_category.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpGet("categories")]
        public IActionResult Index()
        {
            List<Category> categories = _db.Categories
                .Include(category => category.Dishes)
                .OrderBy(category => category.Name)
                .ToList();
            return View(categories);
        }
    }
}
