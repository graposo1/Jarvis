using Microsoft.AspNetCore.Mvc;

namespace JarvisOnTheWeb.Controllers
{
    public class HomeController : Controller
    {
       
        public HomeController()
        {
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}