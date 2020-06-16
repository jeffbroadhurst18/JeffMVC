using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JeffShared
{
	public class CreateUser
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public CreateUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task CreateFirstUser()
		{
			var user = new IdentityUser()
			{
				UserName = "jeff@email.com",
				Email = "jeff@email.com"
			};

			if (await _userManager.FindByEmailAsync(user.Id) == null)
			{ 
			var result = await _userManager.CreateAsync(user, "Password");
			}

			if (!await _roleManager.RoleExistsAsync("admin"))
			{
				await _roleManager.CreateAsync(new IdentityRole("admin"));
				await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("jeff@email.com"), "admin");
			}
			
		}
	}
}
