using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamSyncB.models;
using TeamSyncB.DTOs;
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
            var userReturn = users.Select(user => new
            {
                user.UserId,
                user.Name,
                user.Email,
            });
            return Ok(userReturn);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null) return NotFound();
            
            var userReturn = new
            {
                user.UserId,
                user.Name,
                user.Email,
            };
            return Ok(userReturn);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = await _userService.CreateUser(user);
                var newUserReturn = new
                {
                    newUser.UserId,
                    newUser.Name,
                    newUser.Email
                };
                return Ok(newUserReturn);
            }
            
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.ValidateLogin(loginRequest.Email, loginRequest.Password);
            
            if (user == null) return Unauthorized("Invalid email or password");

            var userReturn = new
            {
                user.UserId,
                user.Name,
                user.Email
            };
            return Ok(userReturn);
        }
    }
}