using SHOP.Data;
using SHOP.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SHOP.Controllers
{
    [Route("v1")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get(
            [FromServices] DataContext context
        )
        {
            var employee = new User {Id = 1, UserName = "Robin", Password = "robin", Role = "employee" };
            var manager = new User {Id = 2, UserName = "Batman", Password = "batman", Role = "manager"};
            var category = new Category { Id = 1, Title = "Informática"};
            var product = new Product { Id = 1, Category = category, Title = "Mouse", Price = 299, Description = "Um mouse gamer top",  };

            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            return Ok(new {
                message = "Todos os cadastros foram efetuados com sucesso!"
            });
        }
    }
}