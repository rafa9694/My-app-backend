using My_app_backend.Models;
using My_app_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace My_app_backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
          public ActionResult<dynamic> Authenticate(User model)
          {
              // Recupera o usuário
              var user =  _userService.GetByName(model.Name);

              // Verifica se o usuário existe
              if (user == null || user.Password != model.Password)
                  return NotFound(new { message = "Usuário ou senha inválidos" });

              // Gera o Token
              var token = TokenService.GenerateToken(user);
              
              // Retorna os dados
              return new
              {
                  user = _userService.Get(user.Id),
                  token = token
              };
          }
    }
}