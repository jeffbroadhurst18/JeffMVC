using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace JeffAPI.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _config;
		private readonly IMapper _mapper;

		public AccountController(IMapper mapper, SignInManager<IdentityUser> signInManager,
			UserManager<IdentityUser> userManager,IConfiguration config)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_config = config;
		}

		public IActionResult Login()
		{
			if (this.User.Identity.IsAuthenticated) //Gets the infor for the current user
			{
				RedirectToAction("Index", "App");
			}
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody]Login login)
		{
			IActionResult response = Unauthorized();
			var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, false);

			if (!result.Succeeded) { return BadRequest(); }

			if (login != null)
			{
				var tokenString = GenerateJSONWebToken(login);
				response = Ok(new { token = tokenString });
			}

			return response;
		}

		private string GenerateJSONWebToken(Login userInfo)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_config["Tokens:Issuer"],
			  _config["Tokens:Audience"],
			  null,
			  expires: DateTime.Now.AddMinutes(120),
			  signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "App");
		}

		

	}
}