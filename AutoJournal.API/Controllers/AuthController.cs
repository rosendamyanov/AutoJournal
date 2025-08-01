﻿using AutoJournal.Authentication.Services.Interfaces;
using AutoJournal.Common.Response;
using AutoJournal.DTOs.Request;
using AutoJournal.DTOs.Response;
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
            ApiResponse<AuthResponseDto> response = await _userService.Register(user);

            if (response.IsSuccess)
            {
                return Ok(response); 
            }

            return BadRequest(response); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDTO user)
        {
            ApiResponse<AuthResponseDto> response = await _userService.Login(user);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshRequestDto refresh)
        {
            ApiResponse<AuthResponseDto> response = await _userService.RefreshToken(refresh);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
