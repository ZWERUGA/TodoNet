using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Dtos.Account;
using Todo.Interfaces;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        SignInManager<AppUser> signInManager
    ) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Проверка на существование пользователя с таким именем или email
                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u =>
                    u.UserName == registerDto.UserName
                );
                if (existingUser is not null)
                    return BadRequest("Пользователь с таким именем уже существует.");

                var existingEmail = await _userManager.Users.FirstOrDefaultAsync(u =>
                    u.Email == registerDto.Email
                );
                if (existingEmail is not null)
                    return BadRequest("Пользователь с таким email уже существует.");

                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (!createdUser.Succeeded)
                    return StatusCode(500, createdUser.Errors);

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                if (!roleResult.Succeeded)
                    return StatusCode(500, roleResult.Errors);

                var token = _tokenService.CreateToken(appUser);
                var csrfToken = Guid.NewGuid().ToString();

                Response.Cookies.Append(
                    "jwt",
                    token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Path = "/",
                        MaxAge = TimeSpan.FromHours(1),
                    }
                );

                Response.Cookies.Append(
                    "csrfToken",
                    csrfToken,
                    new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Path = "/",
                        MaxAge = TimeSpan.FromHours(1),
                    }
                );

                return Ok(new NewUserDto { UserName = appUser.UserName!, Email = appUser.Email! });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.UserName == loginDto.UserName
            );

            if (appUser is null)
                return Unauthorized("Пользователь не найден.");

            var result = await _signInManager.CheckPasswordSignInAsync(
                appUser,
                loginDto.Password,
                false
            );

            if (!result.Succeeded)
                return Unauthorized("Указаны неверные данные.");

            var token = _tokenService.CreateToken(appUser);

            var csrfToken = Guid.NewGuid().ToString();

            Response.Cookies.Append(
                "jwt",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/",
                    MaxAge = TimeSpan.FromHours(1),
                }
            );

            Response.Cookies.Append(
                "csrfToken",
                csrfToken,
                new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/",
                    MaxAge = TimeSpan.FromHours(1),
                }
            );

            return Ok(new NewUserDto { UserName = appUser.UserName!, Email = appUser.Email! });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("csrfToken");

            return Ok(new { message = "Вы успешно вышли из системы " });
        }
    }
}
