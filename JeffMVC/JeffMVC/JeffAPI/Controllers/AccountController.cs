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
using Microsoft.AspNetCore.Cors;

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
			UserManager<IdentityUser> userManager, IConfiguration config)
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
		[EnableCors("AnyGET")]
		[Route("login")]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody]Login login)
		{
			IActionResult response = Unauthorized();
			var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, false);

			if (!result.Succeeded) { return BadRequest(new { message = "Failed to Login" }); }

			if (login != null)
			{
				DateTime expires;
				var tokenString = GenerateJSONWebToken(login, out expires);
				response = Ok(new { token = tokenString, user = login.Username, expires });
			}

			return response;
		}

		[AllowAnonymous]
		[EnableCors("AnyGET")]
		[Route("register")]
		[HttpPost]
		public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
		{

			// Copy data from RegisterViewModel to IdentityUser
			var user = new IdentityUser
			{
				UserName = model.Email,
				Email = model.Email
			};

			try
			{
				var result = await _userManager.CreateAsync(user, model.Password);
				if (!result.Succeeded)
				{
					var errorString = string.Empty;
					foreach (var error in result.Errors)
					{
						errorString += error.Description + Environment.NewLine;
					}
					return BadRequest(new { message = errorString });
				}

				await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
				if (user != null)
				{
					var login = new Login
					{
						Username = model.Email,
						Password = model.Password,
						RememberMe = false
					};
					DateTime expires;
					var tokenString = GenerateJSONWebToken(login, out expires);
					return Ok(new { token = tokenString });
				}
				return BadRequest(new { message = "Failed to login" });
			}
			catch (Exception ex)
			{
				// return error message if there was an exception
				return BadRequest(new { message = ex.Message });
			}
		}

		private string GenerateJSONWebToken(Login userInfo, out DateTime expires)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_config["Tokens:Issuer"],
			  _config["Tokens:Audience"],
			  null,
			  expires: DateTime.Now.AddMinutes(120),
			  signingCredentials: credentials);

			expires = token.ValidTo;
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		[EnableCors("AnyGET")]
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "App");
		}



	}
}