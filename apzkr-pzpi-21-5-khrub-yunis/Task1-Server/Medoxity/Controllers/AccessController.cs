using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Medoxity.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Medoxity.Services;

namespace Medoxity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAccessService _accessService;

        public AccessController(IAccessService accessService)
        {
            _accessService = accessService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            var result = await _accessService.RegisterAsync(model);
            if (!result)
            {
                return BadRequest(new { message = "User with the same Username or Email already exists." });
            }

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            var user = await _accessService.LoginAsync(model);
            if (user == null)
            {
                return Unauthorized(new { message = "Wrong username or password." });
            }

            // Аутентифікація
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, model.Username),
            new Claim("OtherProperties", user.Role),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = model.KeepLoggedIn
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return Ok(new { message = "User logged in successfully." });
        }

        [HttpGet("accessdenied")]
        public IActionResult AccessDenied()
        {
            return Forbid();
        }
    }
}
