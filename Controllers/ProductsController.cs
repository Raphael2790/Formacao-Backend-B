using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOP.Data;
using SHOP.Models;

namespace SHOP.Controllers
{

    //v1 refere-se ao controle de versionamento do controller
    //Quando surgir alterações nesse controlador podemos aumentar esse versionamento
    [Route("v1/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "employee")]
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
        [Authorize(Roles = "employee")]
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
        [Authorize(Roles = "employee")]
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
        [Authorize(Roles = "employee")]
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

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Product>> Put(
            int id,
            [FromBody]Product model,
            [FromServices]DataContext context
        )
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            if(id != model.Id)
            return NotFound(new {message ="Produto não encontrado"});

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new {message = "Produto alterado com sucesso", product = model});
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Product>> Delete(
            int id,
            [FromServices] DataContext context
        )
        {
            try
            {
                var product = await context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
                if(product == null)
                return NotFound(new { message = "Não encontramos nenhum produto correspondente"});

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(new { message = "Produto deletado com sucesso", product = product});
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest(new {message = "Este registro está em uso no momento, tente mais tarde"});
            }
            catch
            {
                return BadRequest(new {message = "Não foi possível deletar o produto, tente mais tarde"});
            }
        }
    }
}
