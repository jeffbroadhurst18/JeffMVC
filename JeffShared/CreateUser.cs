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

		public CreateUser(UserManager<IdentityUser> userManager) => this._userManager = userManager;

		public async Task CreateFirstUser()
		{
			var user = new IdentityUser
			{
				UserName = "jeff@email.com",
				Email = "jeff@email.com"
			};

			var result = await _userManager.CreateAsync(user, "Password_1");
		}
	}
}
