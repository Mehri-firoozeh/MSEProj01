using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace CostcoSU.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        public IActionResult VerticalList()
        {
            ViewData["Message"] = "Verticals page.";

            return View();
        }
        public IActionResult ProjectList()
        {
            ViewData["Message"] = "Your list of projects.";

            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
