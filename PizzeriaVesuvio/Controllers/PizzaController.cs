using PizzeriaVesuvio.Database;
using PizzeriaVesuvio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace PizzeriaVesuvio.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            using (PizzaContext db = new())
            {
                List<PizzaModel> pizze = db.Pizzas.ToList();
                return View(pizze);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext db = new())
            {
                List<CategoryModel> categories = db.Categories.ToList();
                PizzaFormModel model = new();
                model.Pizza = new PizzaModel();
                model.Categories = categories;

                return View("Create", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new())
                {
                    List<CategoryModel> categories = db.Categories.ToList();
                    data.Categories = categories;

                    return View("Create", data);
                }
            }

            using (PizzaContext db = new())
            {
                PizzaModel pizzaToCreate = new();
                pizzaToCreate.Name = data.Pizza.Name;
                pizzaToCreate.CategoryId = data.Pizza.CategoryId;
                pizzaToCreate.Description = data.Pizza.Description;
                pizzaToCreate.Price = data.Pizza.Price;
                pizzaToCreate.ImageUrl = data.Pizza.ImageUrl;
                db.Pizzas.Add(pizzaToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzaContext db = new())
            {
                PizzaModel? pizzaDetails = db.Pizzas.Where(PizzaModel => PizzaModel.Id == id)
                    .Include(PizzaModel => PizzaModel.Category).FirstOrDefault();

                if (pizzaDetails != null)
                {
                    return View("Details", pizzaDetails);
                }
                else
                {
                    return NotFound($"L'articolo con id {id} non è stato trovato!");
                }
            }
        }

        public IActionResult Manage()
        {
            using (PizzaContext db = new())
            {
                List<PizzaModel> pizze = db.Pizzas.ToList();
                return View(pizze);
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext db = new())
            {
                PizzaModel? pizzaToEdit = db.Pizzas.Where(PizzaModel => PizzaModel.Id == id).FirstOrDefault();

                if (pizzaToEdit != null)
                {
                    List<CategoryModel> categories = db.Categories.ToList();
                    PizzaFormModel model = new();
                    model.Pizza = pizzaToEdit;
                    model.Categories = categories;

                    return View("Update", model);
                }
                else
                {
                    return NotFound($"L'articolo con id {id} non è stato trovato!");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new())
                {
                    List<CategoryModel> categories = db.Categories.ToList();
                    data.Categories = categories;
                    return View("Update", data);
                }
            }

            using (PizzaContext db = new())
            {
                PizzaModel? pizzaToModify = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToModify != null)
                {

                    pizzaToModify.Name = data.Pizza.Name;
                    pizzaToModify.CategoryId = data.Pizza.CategoryId;
                    pizzaToModify.Description = data.Pizza.Description;
                    pizzaToModify.Price = data.Pizza.Price;
                    pizzaToModify.ImageUrl = data.Pizza.ImageUrl;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    return NotFound("L'articolo da modificare non esiste!");
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (PizzaContext db = new())
            {
                PizzaModel? pizzaToDelete = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                else
                {
                    return NotFound("Non ho torvato l'articolo da eliminare");

                }
            }
        }
    }
}
