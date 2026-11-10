using Microsoft.AspNetCore.Mvc;

namespace Top5.ViewComponents
{
    // A view component: a partial with its own C# code. The layout calls it on every page.
    public class OpenNowViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            int hour_now = DateTime.Now.Hour;
            bool is_open = hour_now >= 11 && hour_now < 22;
            return View(is_open);
        }
    }
}
