using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndeesTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserLogin _userLogin;

        public UserController(IUserLogin userLogin)
        {
            _userLogin = userLogin ?? throw new ArgumentNullException(nameof(userLogin));
        }
        [HttpGet("LoggedUserDetails")]
        public async Task<IActionResult> LoggedUserDetails(Guid userId)
        {
            if (userId == null)
            {
                return BadRequest("userId cannot be null.");
            }

            var userDetails = await _userLogin.GetLoggedUserDetails(userId);

            if (userDetails == null)
            {
                return NotFound("User not found.");
            }

            return Ok(userDetails);
        }
    }
}
