using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOP.Data;
using SHOP.Models;

namespace SHOP.Controllers
{

    [Route("products")]
    public class ProductsController : ControllerBase
    {
    [HttpGet]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context) 
        {
            try
            {
                var products = await context
                .Products
                //Fazer um join com a tabela categoria para buscar somente produtos com categoria cadastrada
                .Include(x => x.Category)
                .AsNoTracking()
                //Todos tipos de manipulação dos dados devem ser feito antes de Listar
                //Ex: OrderBy, Sort, Filter, etc
                .ToListAsync();

                if(products.Count == 0)
                return NotFound(new { message = "Não foram encontradas produtos registrados"});

                return Ok(products);
            }
            catch (System.Exception)
            {
                return BadRequest(new {message = "Desculpe, aconteceu um erro, tente novamente mais tarde"});
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(
            int id, 
            [FromServices] DataContext context
            ) 
        {
            try
            {
                var product = await context
                .Products
                //Fazer um join com a tabela categoria para buscar somente produtos com categoria cadastrada
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

                if(product == null)
                return NotFound(new { message = "Não foi encontrado o produto registrado"});

                return Ok(product);
            }
            catch (System.Exception)
            {
                return BadRequest(new {message = "Desculpe, aconteceu um erro, tente novamente mais tarde"});
            }
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategoryId(
            int id, 
            [FromServices] DataContext context
            ) 
        {
            try
            {
                var products = await context
                .Products
                //Fazer um join com a tabela categoria para buscar somente produtos com categoria cadastrada
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();

                if(products.Count == 0)
                return NotFound(new { message = "Não foram encontrados produtos registrados nesta categoria"});

                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest(new {message = "Desculpe, aconteceu um erro, tente novamente mais tarde"});
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
            [FromBody] Product model,
            [FromServices] DataContext context
            )
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Não foi possível concluir a operação, tente mais tarde"});
            }
        }
    }
}
