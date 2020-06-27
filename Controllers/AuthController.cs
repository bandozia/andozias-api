using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using api.DAL.Models;
using api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("mail-check/{email}")]
        public async Task<bool> CheckEmailExists(string email)
        {
            return await _userService.CheckUserExists(email);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User user)
        {
            try
            {
                string token = await _userService.AuthenticateUser(user.Email.ToLower(), user.Password);
                return Ok(token);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User newUser)
        {
            try
            {
                await _userService.CreateNewUser
                (
                    newUser.Email,
                    newUser.Name,
                    newUser.Password,
                    null,
                    UserService.DefaultRolesNames
                );
            }
            catch (Exception err)
            {
                Console.WriteLine($"erro ao criar usuario: {err.Message}|{err.GetType()}");
            }
            return Ok("ok");
        }

    }
}
