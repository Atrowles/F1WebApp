using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using F1WebApp.Models;
using Microsoft.Extensions.Configuration;

namespace F1WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            //eaxample how hot to read settings file
            var testvalue = _configuration["TestValue"];
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }

        public IActionResult Details(int Id)
        {
            ViewData["Id"] = Id;

            return View();
        }

        public IActionResult Update(int Id)
        {
            ViewData["Id"] = Id;

            return View();
        }

        public IActionResult Add()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
