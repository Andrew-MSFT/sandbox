using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreConfigTest.Controllers
{
    public class HomeController : Controller
    {
        Configuration.ConfigValues _config;
        Configuration.SecretValues _secrets;

        public HomeController(Configuration.ConfigValues config = null, Configuration.SecretValues secrets = null)
        {
            _config = config ?? Configuration.Configuration.Config;
            _secrets = secrets ?? Configuration.Configuration.Secrets;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _secrets.SecretA;
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
