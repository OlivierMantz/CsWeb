using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Models;
using BackEnd.Models.DTOs;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

// based on https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Auth0ManagementService _auth0ManagementService;

        public UserController(Auth0ManagementService auth0ManagementService)
        {
            _auth0ManagementService = auth0ManagementService;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(string userId)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == userId || User.IsInRole("Administrator"))
            {

                var user = await _auth0ManagementService.GetUserAsync(userId);
                return Ok(user);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == userId || User.IsInRole("Administrator"))
            {
                //await _auth0ManagementService.DeleteUserAsync(userId);
                return Ok("User deleted succesfully");
            }
            else
            {
                return Forbid();
            }
        }
    }
}