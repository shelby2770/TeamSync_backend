using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamSyncB.models;
using TeamSyncB.services;

namespace TeamSyncB.controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = await _userService.CreateUser(user);
                return CreatedAtAction(nameof(GetUser), new { userId = newUser.UserId }, newUser);
            }
            
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}