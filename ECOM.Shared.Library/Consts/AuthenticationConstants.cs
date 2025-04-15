namespace ECOM.Shared.Library.Consts
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
