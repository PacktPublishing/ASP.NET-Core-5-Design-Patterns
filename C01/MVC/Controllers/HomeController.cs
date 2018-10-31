using C01.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace C01.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public void ActionWithoutResult()
        {
            var youCanSetABreakpointHere = "";
        }

        public IActionResult ActionWithSomeInput(int id)
        {
            var model = id;
            return View(model);
        }

        public IActionResult ActionWithSomeInputAndAModel(int id)
        {
            var model = new SomeModel
            {
                SelectedId = id,
                Title = "This title was set in HomeController!"
            };
            return View(model);
        }
    }
}