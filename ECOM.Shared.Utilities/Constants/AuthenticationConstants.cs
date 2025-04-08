using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.Shared.Utilities.Constants
{
	public static class AuthenticationConstants
	{
		public static class Role
		{
			public const string Admin = "Admin";
			public const string Default = "Default";
			public const string Contributor = "Contributor";
		}

		public static class ClaimType
		{
			public const string Admin = "Admin";
		}

		public static class ClaimValue
		{
			public const string CanAddUser = "CanAddUser";
			public const string CanAddAdminUser = "CanAddAdminUser";
		}
	}
}
