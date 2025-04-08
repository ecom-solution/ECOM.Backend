using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.App.DTOs.Modules.Authentication.Users
{
	public class UserSignIn
	{
		public string UserName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}

	public class UserSignedIn
	{
		public Guid Id { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string TimeZoneId { get; set; } = string.Empty;
		public string Currency { get; set; } = string.Empty;
		public string Language { get; set; } = string.Empty;
		public string AccessToken { get; set; } = string.Empty;
	}
}
