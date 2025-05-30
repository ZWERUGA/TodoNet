using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Extensions;
using Todo.Models;

namespace Todo.Services
{
    public class UserService(UserManager<AppUser> userManager)
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<(AppUser?, IActionResult?)> GetCurrentUserAsync(
            ClaimsPrincipal userClaims
        )
        {
            var username = userClaims.GetUsername();
            if (username is null)
                return (null, new UnauthorizedObjectResult("Требуется авторизация."));

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                return (null, new NotFoundObjectResult("Пользователь не найден."));

            return (appUser, null);
        }
    }
}
