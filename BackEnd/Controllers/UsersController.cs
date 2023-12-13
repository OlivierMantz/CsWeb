using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.Models.DTOs;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BackEnd.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using PostAPI.Utilities;
using Microsoft.AspNetCore.Authorization;

// based on https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private static UserDTO UserToDto(User user)
        {
            var userDto = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
            return userDto;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            var userDtos = users.Select(UserToDto).ToList();
            return Ok(userDtos);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid id parameter. The id must be a positive number.");
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userDto = UserToDto(user);
            return Ok(userDto);
        }

        // GET: api/Users/name/john
        [Authorize]
        [HttpGet("name/{name}")]
        public async Task<ActionResult<UserDTO>> GetUserByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Invalid name parameter. The name must not be null or empty.");
            }

            var user = await _userService.GetUserByNameAsync(name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(UserToDto(user));
        }

        //// TODO Patch

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        //{
        //    if (Validator.CheckInputInvalid(userDto))
        //    {
        //        return Problem("One or more invalid inputs");
        //    }

        //    var user = new User
        //    {
        //        Name = userDto.Name,
        //        Email = userDto.Email,
        //        Password = userDto.Password,
        //        Role = "User"
        //    };

        //    await _userService.PostUserAsync(user);

        //    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, UserToDto(user));
        //}

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserDTO userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("The provided id parameter does not match the user's id.");
            }

            var existingUser = await _userService.GetUserByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = userDto.Name;
            existingUser.Email = userDto.Email;
            existingUser.Password = userDto.Password;

            await _userService.PutUserAsync(existingUser);

            return NoContent();
        }

        // DELETE: api/Users/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}