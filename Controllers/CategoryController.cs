using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOP.Data;
using SHOP.Models;

namespace SHOP.Controllers
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        //define uma rota filha dentro do controlador da API
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context) 
        {
            try
            {
                var categories = await context.Categories.AsNoTracking().ToListAsync();
                if(categories.Count == 0)
                return NotFound(new { message = "Não foram encontradas categorias registradas"});

                return Ok(categories);
            }
            catch (System.Exception)
            {
                return BadRequest(new {message = "Desculpe, aconteceu um erro, tente novamente mais tarde"});
            }
        }

        //rota filha com parametro com restrição através de tipagem id:int
        //estado retornado caso o parametro não seja um inteiro será 404 Not Found
        [HttpGet]
        [Route("{id:int}")]
        //Criando Tasks podemos trabalhar com multiplas threads e assincronismo (paralelismo)
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context) 
        {
            try
            {
                //AsNoTracking faz com que o entity traga elementos mais rapidamente para leitura
                //Tirando por exemplo dados de ultima alteração e criação
                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if(category == null)
                return NotFound(new {message = "Não conseguimos encontrar a categoria desejada"});

                return Ok(category);
            }
            catch (System.Exception)
            {
                
                return BadRequest(new {message = "Desculpe, aconteceu um erro tente mais tarde"});
            }
        }

        [HttpPost]
        [Route("")]
        //Action Result permite devolver como respostas status code e um tipo definido quando necessário
        public async Task<ActionResult<Category>> Post([FromBody]Category model, [FromServices] DataContext context) 
        {
            //ModelState é um propriedade do Core.Mvc que válida o obj de acordo com o model definido 
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception ex)
            {   
                System.Console.WriteLine(ex.Message);
                return BadRequest(new {message = "Não foi possível criar a categoria, tente novamente"});
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(
            int id,
            [FromBody] Category model,
            [FromServices] DataContext context
            ) 
        {   
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            
            if(model.Id != id)
            return NotFound(new { message = "Não encontramos a categoria desejada"});
            
            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new {message = "Este registro está sendo atualizado"});   
            }
            catch (Exception)
            {
                return BadRequest(new {message = "Aconteceu um erro ao atualizar categoria, tente mais tarde"});
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Delete(
            int id,
            [FromServices] DataContext context
            )
        {
            //filtro para encontrar o elemento
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category == null)
            return NotFound(new {message = "Categoria não encontrada"});

            try
            {
            //contexto (todo) -> onde (tabela) -> metodo (ação) -> elemento  
            context.Categories.Remove(category);
            //efetuar a que está em memória para o banco
            await context.SaveChangesAsync();
            //status code do action result
            return Ok(new {message = "Categoria excluida com sucesso"});  
            }
            catch (System.Exception)
            {
                
                return BadRequest(new { message = "Aconteceu algum erro ao tentar excluir, tente mais tarde"});
            }
        }
    } 
}

