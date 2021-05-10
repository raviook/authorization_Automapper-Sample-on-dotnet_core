using System.Threading.Tasks;
using dotnetcore_rpg.Data;
using dotnetcore_rpg.DTO.User;
using dotnetcore_rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnetcore_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto newuser)
        {
            var response = await _authRepository.Register(new User() { UserName = newuser.UserName }, newuser.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var response = await _authRepository.Login(login.UserName,login.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}