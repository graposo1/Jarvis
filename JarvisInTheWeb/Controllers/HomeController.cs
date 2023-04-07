using Microsoft.AspNetCore.Mvc;

namespace JarvisInTheWeb.Controllers
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