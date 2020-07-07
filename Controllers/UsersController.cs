using My_app_backend.Models;
using My_app_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace My_app_backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Student")]
        public ActionResult<List<UserDto>> Get() =>
            _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        [Authorize(Roles = "Admin,Student")]
        public ActionResult<UserDto> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<User> Create(User user)
        {
            _userService.Create(user);

            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        [HttpPut("{id:length(24)}")]
        [Authorize]
        public IActionResult Update(string id, User userIn)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            if(userIn == null || userIn.Name == null || userIn.Password == null)
            {
                return BadRequest(new { message = "Ùsuário e/ou senha estão vazios" });
            }

            if(User.IsInRole("Admin"))
            {
                _userService.Update(id, userIn);
            } else 
            {
                var UserAutenticated = User.Identity.Name;

                if(_userService.GetByName(UserAutenticated).Id == id) 
                {
                    if(userIn.Admin) 
                    {
                        return BadRequest(new { message = "Você não possui autorização para transformar um o úsuário em administrador" });
                    }
                    _userService.Update(id, userIn);
                } else 
                {
                    return BadRequest(new { message = "Você não possui autorização para atualizar o úsuário" });
                }
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Remove(user.Id);

            return NoContent();
        }
    }
}