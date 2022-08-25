using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using ToDo_Server.Application.Interfaces;
using ToDo_Server.Data.Mapper;
using ToDo_Server.Data.Models;

namespace ToDo_Server.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _token;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(AppLoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                await SetRefreshToken(user);
                return CreateUserObject(user);
            }
            return Unauthorized();
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(AppRegisterDto registerDto)
        {
            var taken = await CheckIfUserPropsAreTaken(registerDto);
            if (taken.Item1 == "Fail") return BadRequest(taken.Item2);
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await SetRefreshToken(user);
                return CreateUserObject(user);
            }
            return BadRequest("Problem registering user . . . ");
        }

        public async Task<Tuple<string, string>> CheckIfUserPropsAreTaken(AppRegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
                return new("Fail", "Email Taken!");

            if (await _userManager.Users.AnyAsync(x => x.DisplayName == registerDto.DisplayName))
                return new("Fail", "DisplayName Taken!");

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
                return new("Fail", "UserName Taken!");

            return new("Success", "move On");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            await SetRefreshToken(user);
            return CreateUserObject(user);
        }

        private UserDto CreateUserObject(AppUser user) => new UserDto
        {
            DisplayName = user.DisplayName,
            UserName = user.UserName,
            Token = _token.CreateToken(user.UserName, user.Id, user.Email)
        };

        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null) return Unauthorized();

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
            if (oldToken != null && !oldToken.IsActive) return Unauthorized();
            return CreateUserObject(user);

        }

        public async Task SetRefreshToken(AppUser appUser)
        {
            try
            {
                var refreshToken = _token.GenerateRefreshToken();

                appUser.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(appUser);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                };

                Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }


    }
}