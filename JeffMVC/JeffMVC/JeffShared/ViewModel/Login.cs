using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JeffShared.ViewModel
{
	public class Login
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
