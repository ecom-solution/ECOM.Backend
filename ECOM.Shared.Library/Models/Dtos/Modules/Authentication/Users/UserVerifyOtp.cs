﻿namespace ECOM.Shared.Library.Models.Dtos.Modules.Authentication.Users
{
	public class UserVerifyOtp
	{
		public string UserName { get; set; } = string.Empty;
		public string SecretKey { get; set; } = string.Empty;
		public string OtpCode { get; set; } = string.Empty;
	}
}
