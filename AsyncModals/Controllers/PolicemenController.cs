using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncModals.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncModals.Controllers
{
    public class PolicemenController : Controller
    {
        [HttpGet]
        public IActionResult Create(string id)
        {
            ViewBag.ShepardId = id;
            return PartialView(new Policeman());
        }
        [HttpPost]
        public IActionResult Update([Bind] Policeman policeman)
        {
            (Sheep sheep, Shepard shepard, Policeman police) = Sheep.FindAPoliceman(policeman.Name);
            ViewBag.SheepId = sheep.Id;
            return PartialView("Create", police);
        }
        [HttpPost]
        public IActionResult Modify(string id, Policeman policeman)
        {
            (Sheep sheep, Shepard shepard) = Sheep.FindAShepard(id);
            if (sheep != null)
            {
                shepard.Policemen.Add(policeman);
                return PartialView("../Shepard/_ListPolicemen", shepard.Policemen);
            }
            return RedirectResult("Index", "Home");
        }
        public IActionResult Delete(string id)
        {
            (Sheep sheep, Shepard shepard, Policeman policeman) = Sheep.FindAPoliceman(id);
            if (sheep != null)
            {
                shepard.Policemen.Remove(policeman);
                //Sheep.UpdateASheep(sheep);
                return PartialView("../Shepard/_ListPolicemen", sheep.Shepards);
            }
            return RedirectResult("Index", "Home");
        }
        public IActionResult RedirectResult(string action, string controller = null, object values = null) => BadRequest($"document.location = '{Url.Action(action, controller, values)}';");
    }
}