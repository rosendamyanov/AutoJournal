using AutoJournal.DTOs.Request;
using AutoJournal.Services.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutoJournal.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestDTO user)
        {
            bool response = await _userService.Register(user);

            return  response ? Ok(response) : BadRequest();
        }
    }
}
