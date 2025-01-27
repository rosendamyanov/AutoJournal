using AutoJournal.DTOs.Request;
using AutoJournal.Services.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutoJournal.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;

        public AuthController(IAuthService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequestDTO user)
        {
            bool response = await _userService.Register(user);

            return  response ? Ok(response) : BadRequest();
        }
    }
}
