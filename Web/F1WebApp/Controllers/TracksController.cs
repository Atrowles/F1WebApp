using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace F1WebApp.Controllers
{
    public class TracksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int Id)
        {
            ViewData["Id"] = Id;

            return View();
        }

        public IActionResult Add()
        {

            return View();
        }
    }
}