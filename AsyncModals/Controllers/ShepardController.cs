using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncModals.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncModals.Controllers
{
    public class ShepardController : Controller
    {
        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.SheepId = id;
            return PartialView(new Shepard());
        }
        [HttpPost]
        public IActionResult Update([Bind] Shepard shepard)
        {
            (Sheep sheep, Shepard shepard2) = Sheep.FindAShepard(shepard.Name);
            ViewBag.SheepId = sheep.Id;
            return PartialView("Create", shepard2);
        }
        [HttpPost]
        public IActionResult Modify(int id, Shepard shepard)
        {
            Sheep sheep = Sheep.FindASheep(id);
            if (sheep != null)
            {
                sheep.Shepards.Add(shepard);
                Sheep.UpdateASheep(sheep);
                return PartialView("../Home/_ListShepard", sheep.Shepards);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}