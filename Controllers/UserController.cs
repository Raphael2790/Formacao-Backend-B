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
    [Route("users")]
    public class UserController : ControllerBase
    {
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
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Houve algum problema ao tentar fazer o cadastro, tente mais tarde"});
            }
        }

        [HttpPost]
        [Route("login")]
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
            return new
            {
                userId = user.Id,
                userName = user.UserName,
                userRole = user.Role,
                token=  token
            };
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