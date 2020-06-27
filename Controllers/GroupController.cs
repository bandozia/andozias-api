using System;
using System.Linq;
using System.Threading.Tasks;
using api.DAL.Models;
using api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/groups")]
    public class GroupController : Controller
    {
        private readonly UserService _userService;


        public GroupController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        [Authorize(Policy = "HasNoGroup")]
        public async Task<IActionResult> CreateGroup([FromBody] Group newGroup)
        {
            var user = await _userService.GetUser(User);
            var group = await _userService.CreateNewGroup(newGroup.Name);
            await _userService.AssignUserToGroup(user, group);
            return Ok("ok");
        }
    }
}
