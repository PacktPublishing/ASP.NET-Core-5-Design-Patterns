using Microsoft.AspNetCore.Mvc;

namespace C01.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index => View();
    }
}