using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHOP.Data;
using SHOP.Models;
using SHOP.Services;

namespace SHOP.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> GetAction(
            [FromServices] DataContext context
        )
        {   
            try
            {
                var users = await context.Users
                .AsNoTracking()
                .ToListAsync();

                if(users.Count == 0)
                return NotFound(new {message = "Não foram encotrados usuários cadastrados"});

                return Ok(users);
            }
            catch
            {
                return BadRequest(new { message = "Desculpe mas não foi possível realizar a consulta, tente mais tarde"});
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> GetById(
            int id,
            [FromServices] DataContext context
        )
        {
            try
            {
                var user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

                if(user == null)
                return NotFound(new { messsage = "Usuário não encontrado"});

                return Ok(user);
            }
            catch
            {
                return BadRequest(new {message = "Desculpe mas não foi possível retornar o usuário, tente mais tarde"});
            }
        }
        
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromServices] DataContext context,
            [FromBody] User model)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            try
            {
                model.Role = "employee";
                context.Users.Add(model);
                await context.SaveChangesAsync();
                model.Password = "***********";
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Houve algum problema ao tentar fazer o cadastro, tente mais tarde"});
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        //Ao usar o dynamic podemos retornar tipos dinâmicos ao invès de tipados
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] DataContext context,
            [FromBody] User model
        )
        {
            var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Password == model.Password && x.UserName == model.UserName)
            .FirstOrDefaultAsync();

            if(user == null)
            return NotFound(new {message = "Usuário ou senha inválidos"});

            var token = TokenService.GenerateToken(user);
            user.Password = "***********";
            return new
            {
                user = user,
                token=  token
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(
            int id,
            [FromBody] User model,
            [FromServices] DataContext context
        )
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            if(id != model.Id)
            return NotFound(new {message = "Usuário não encontrado"});

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new {message = "Usuário alterado com sucesso", user = model});
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new {message = "O registro está sendo alterado, tente mais tarde"});
            }
            catch
            {
                return BadRequest(new {message = "Desculpe mas não foi possível concluir a alteração, tente mais tarde"});
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<User>> Delete(
            int id,
            [FromServices] DataContext context
        )
        {
            try
            {
                var user = await context.Users
                .FirstOrDefaultAsync();
                if(user == null)
                return NotFound(new {message = "Usuário não encontrado"});
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return Ok(new {message = "Usuário deletado com sucesso", user = user});
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest(new {message = "Este cadastro está em uso no momento, tente mais tarde"});
            }
            catch
            {   
                return BadRequest(new { message = "Não foi possível deletar o usuário, tente novamente mais tarde"});
            }
        }
    }
}