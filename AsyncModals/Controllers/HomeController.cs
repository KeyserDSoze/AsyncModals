using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AsyncModals.Models;

namespace AsyncModals.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(Sheep.List());
        }
        [HttpGet]
        public IActionResult Create()
        {
            Sheep.CreateRandomSheep();
            return View("Index", Sheep.List());
        }
        [HttpGet]
        public IActionResult Edit(int sheepId)
        {
            return View(Sheep.FindASheep(sheepId));
        }
        [HttpPost]
        public IActionResult Edit(Sheep sheep)
        {
            //Sheep.UpdateASheep(sheep);
            //return Ok();
            return RedirectResult("Index");
        }
        public IActionResult RedirectResult(string action, string controller = null, object values = null)
        {
            return BadRequest($"document.location = '{Url.Action(action, controller, values)}';");
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
        public IActionResult ListShepard(List<Shepard> shepards)
        {
            return PartialView("_ListShepard", shepards);
        }
    }
}
