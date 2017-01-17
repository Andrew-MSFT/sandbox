using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreConfigTest.Controllers
{
    public class HomeController : Controller
    {
        Configuration.Settings _config;
        Configuration.Secrets _secrets;

        public HomeController(Configuration.Settings config = null, Configuration.Secrets secrets = null)
        {
            _config = config ?? Configuration.ConfigurationManager.SettingValues;
            _secrets = secrets ?? Configuration.ConfigurationManager.SecretValues;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _secrets["SecretA"];
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = _config["Logging:LogLevel:Default"];

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
