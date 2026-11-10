using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Top5.Data;
using Top5.Models;

namespace Top5.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _db;

        public MenuController(AppDbContext db)
        {
            _db = db;
        }

        private void LoadCategories()
        {
            List<Category> categories = _db.Categories
                .OrderBy(category => category.Name)
                .ToList();
            ViewBag.Categories =
                new SelectList(categories, "Id", "Name");
            
        }


        // Reached through the "menu" route in Program.cs: /menu, /menu/friday
        public IActionResult Show(string day, bool only_spicy, string? search_text, string? sort_by)
        {
            ViewData["Title"] = "Menu";
            ViewBag.Day = day;

            // The kitchen is closed on Sunday: an empty list, not a missing one
            if ((day ?? string.Empty).ToLower() == "sunday")
            {
                return View(new List<Dish>());
            }

            IQueryable<Dish> query = _db.Dishes.Include(d => d.Category);



            if (only_spicy)
            {
                query = query.Where(dish => dish.IsSpicy);
            }
            if (!string.IsNullOrWhiteSpace(search_text))
            {
                query = query.Where(dish => dish.Name.Contains(search_text));
            }

            if (sort_by == "price")
            {
                query = query.OrderBy(dish => dish.Price);
            }
            else if (sort_by == "name")
            {
                query = query.OrderBy(dish => dish.Name);
            }

            return View(query.ToList());
        }

        // Stretch: one page per dish. /menu/dish/2 shows Escovitch fish. /menu/dish/9 is a 404.
        [AllowAnonymous]
        [HttpGet("menu/dish/{id:int}")]
        public IActionResult Details(int id)
        {
            var each_dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
            if (each_dish != null)
            {
                ViewData["Title"] = each_dish.Name;
                return View(each_dish);
            }
            return NotFound();
        }

        // Stretch: Order is a POST now, from a form button. Typing /menu/order/2 in the
        // address bar (a GET) no longer orders anything.
        [AllowAnonymous]
        [HttpPost("menu/order/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Order(int id)
        {
            var dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null) return NotFound();

            if (User.Identity?.IsAuthenticated == true)
            {
                TempData["Message"] = $"Added {dish.Name} to the order for {User.Identity.Name}";
            }
            else
            {
                TempData["Message"] = $"Added {dish.Name} to your order";
            }

            return RedirectToAction("Show");
        }

        [Authorize(Policy = "CanEditMenu")]
        [HttpGet("menu/secret")]
        public IActionResult New()
        {
            ViewData["Title"] = "Add a dish";
            LoadCategories();
            return View(new Dish());
        }

        [Authorize(Policy = "CanEditMenu")]
        [HttpPost("menu/add")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Dish new_dish)
        {
            // Stretch: a rule the attributes can't express. Is the name taken?
            if (!string.IsNullOrEmpty(new_dish.Name) && _db.Dishes.Any(d => d.Name.ToLower() == new_dish.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "That dish is already on the menu");
            }

            // A rule failed: show the same form again, with what they typed and the errors
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Add a dish";
                LoadCategories();
                return View("New", new_dish);   // the form lives in New.cshtml
            }

            _db.Dishes.Add(new_dish);
            _db.SaveChanges();

            TempData["Message"] = $"{new_dish.Name} is on the menu";
            return RedirectToAction("Show");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("menu/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            Dish? dish = _db.Dishes.Find(id);
            if (dish == null)
            {
                return NotFound();
                
            }

            return View(dish);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("menu/DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Dish? dish = _db.Dishes.Find(id);
            if (dish == null)
            {
                return NotFound();
            }
            _db.Dishes.Remove(dish);
            _db.SaveChanges();
            TempData["Message"] = $"{dish.Name} has been deleted from the menu";
            return RedirectToAction("Show");
        }

        [Authorize(Policy = "CanEditMenu")]
        [HttpGet("menu/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var each_dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
            if (each_dish != null)
            {
                ViewData["Title"] = "Edit a dish";
                LoadCategories();
                return View(each_dish);
            }
            return NotFound();
        }

        [Authorize(Policy = "CanEditMenu")]
        [HttpPost("menu/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Dish changed_dish)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Edit a dish";
                LoadCategories();
                return View(changed_dish);
            }

            var each_dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
            if (each_dish != null)
            {
                each_dish.Name = changed_dish.Name;
                each_dish.Price = changed_dish.Price;
                each_dish.IsSpicy = changed_dish.IsSpicy;
                each_dish.CategoryId = changed_dish.CategoryId;
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while saving the dish.");
                    return View(changed_dish);
                }

                TempData["Message"] = $"Saved {each_dish.Name}";
                return RedirectToAction("Show");
            }
            return NotFound();
        }

        
        
    }
}
