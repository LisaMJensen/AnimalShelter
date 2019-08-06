using Microsoft.AspNetCore.Mvc;
using AnimalShelter.Models;
using System.Collections.Generic;

namespace AnimalShelter.Controllers
{
    public class ItemsController : Controller
    {

        [HttpGet("/animals/new")]
        public ActionResult New(int animalId)
        {
            Animal animal = Animal.Find(animalId);
            return View(animal);
        }

        [HttpGet("/animals/{animalId}")]
        public ActionResult Show(int animalId)
        {
            Animal animal = Animal.Find(animalId);
            Dictionary<string, object> model = new Dictionary<string, object>();
            model.Add("animal", animal);
            return View(model);
        }

        [HttpPost("/items/delete")]
        public ActionResult DeleteAll()
        {
            Animal.ClearAll();
            return View();
        }

    }
}