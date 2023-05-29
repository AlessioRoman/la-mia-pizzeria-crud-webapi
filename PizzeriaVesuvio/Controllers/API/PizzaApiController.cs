using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaVesuvio.Database;
using PizzeriaVesuvio.Models;

namespace PizzeriaVesuvio.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaAPIController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetArticles()
        {
            using (PizzaContext db = new())
            {
                List<PizzaModel> pizze = db.Pizzas.ToList();
                return Ok(pizze);
            }
        }

        [HttpGet("{id}")]
        public IActionResult SearchById(int id)
        {
            using (PizzaContext db = new())
            {
                PizzaModel? pizzaToSearch = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToSearch != null)
                {
                    return Ok(pizzaToSearch);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet]
        public IActionResult SearchByName(string name)
        {
            using (PizzaContext db = new())
            {
                PizzaModel? pizzaToSearch = db.Pizzas.Where(pizza => pizza.Name.Contains(name)).FirstOrDefault();

                if (pizzaToSearch != null)
                {
                    return Ok(pizzaToSearch);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] PizzaModel article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                using (PizzaContext db = new PizzaContext())
                {
                    db.Pizzas.Add(article);
                    db.SaveChanges();

                    return Ok();
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                PizzaModel? pizzaToDelete = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound("Non ho torvato l'articolo da eliminare");

                }
            }
        }
    }
}