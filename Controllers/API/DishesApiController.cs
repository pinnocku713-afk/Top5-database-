using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Top5.Data;
using Top5.Models;

namespace Top5.Controllers.API
{
    [ApiController]
    [Route("api/dishes")]
    public class DishesApiController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DishesApiController(AppDbContext db)
        {
            _db = db;
        }

        private static DishDto ToDto(Dish dish)
        {
            DishDto dto = new DishDto();
            dto.Id = dish.Id;
            dto.Name = dish.Name;
            dto.Price = dish.Price;
            dto.IsSpicy = dish.IsSpicy;
            dto.CategoryId = dish.CategoryId;
            if (dish.Category != null)
            {
                dto.CategoryName = dish.Category.Name;
            }
            return dto;
        }

        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Dish> dishes = _db.Dishes
                .Include(dish => dish.Category)
                .OrderBy(dish => dish.Name)
                .ToList();

            List<DishDto> result = new List<DishDto>();
            foreach (Dish dish in dishes)
            {
                result.Add(ToDto(dish));
            }
            return Ok(result);   // 200 + JSON
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetOne(int id)
        {
            Dish? dish = _db.Dishes
                .Include(each_dish => each_dish.Category)
                .FirstOrDefault(each_dish => each_dish.Id == id);
            if (dish == null)
            {
                return NotFound();     // 404
            }
            return Ok(ToDto(dish));    // 200
        }

        public IActionResult GetAll(string? search, bool? spicy)
        {
            IQueryable<Dish> query = _db.Dishes.Include(dish => dish.Category);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(dish => dish.Name.Contains(search));
            }
            if (spicy != null)
            {
                query = query.Where(dish => dish.IsSpicy == spicy);
            }
            List<Dish> dishes = query.OrderBy(dish => dish.Name).ToList(); 

            return Ok(dishes.Select(dish => ToDto(dish)).ToList());


        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Dish? dish = _db.Dishes.Find(id);
            if (dish == null)
            {
                return NotFound();      
            }

            _db.Dishes.Remove(dish);
            _db.SaveChanges();
            return NoContent();        
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult Create(DishInput input)
        {



            Dish dish = new Dish();
            dish.Name = input.Name;
            dish.Price = input.Price;
            dish.IsSpicy = input.IsSpicy;
            dish.CategoryId = input.CategoryId;

            _db.Dishes.Add(dish);
            try
            {
                _db.SaveChanges();      // INSERT INTO Dishes ...
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "That dish is already on the menu" });
            }

            _db.Entry(dish).Reference(saved_dish => saved_dish.Category).Load();
            return CreatedAtAction(nameof(GetOne), new { id = dish.Id }, ToDto(dish));
        }



    }


}


