using Microsoft.AspNetCore.Mvc;

namespace Top5.Controllers
{
    public class Top5Controller : Controller
    {
        // One list for the whole controller, so every action can read it
        private string[] top_five_items = { "Ackee and saltfish", "Curry goat", "Jerk chicken", "Oxtail", "Fried dumpling" };

        public IActionResult Index()
        {
            return View();
        }

        // /rank shows #1 (the =1 default). /rank/1 to /rank/5 show that item. Anything else is a 404.
        [HttpGet("rank/{id:int:range(1,5)=1}")]
        public IActionResult Number(int id)
        {
            ViewData["Item"] = $"#{id}: {top_five_items[id - 1]}";
            return View();
        }

        [HttpGet("about-my-list")]
        public IActionResult About()
        {
            return View();
        }

        // /dish/jerk finds "Jerk chicken". /dish/pizza says it is not on the list.
        [HttpGet("dish/{name:alpha}")]
        public IActionResult Dish(string name)
        {
            for (int i = 0; i < top_five_items.Length; i++)
            {
                if (top_five_items[i].ToLower().Contains(name.ToLower()))
                {
                    ViewData["Item"] = $"#{i + 1}: {top_five_items[i]}";
                    return View();
                }
            }
            ViewData["Item"] = $"{name} is not on my list";
            return View();
        }

        // Catch-all: /dish/r2d2, /dish/jerk/chicken, /dish
        [HttpGet("dish/{**anything}")]
        public IActionResult NotOnList(string anything)
        {
            ViewData["Asked"] = anything;
            return View();
        }
    }
}
