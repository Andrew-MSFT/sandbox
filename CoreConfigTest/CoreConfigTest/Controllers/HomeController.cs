using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreConfigTest.Controllers
{
    public class HomeController : Controller
    {
        Configuration.Configuration _config;

        public HomeController(Configuration.Configuration config = null)
        {
            _config = config ?? Configuration.Configuration;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = Configuration.Configuration.Secrets.SecretA;
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
