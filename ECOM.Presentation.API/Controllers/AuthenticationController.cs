using ECOM.App.Services.Interfaces;
using ECOM.Shared.Library.Models.Dtos.Common;
using ECOM.Shared.Library.Models.Dtos.Modules.Authentication.Users;
using Microsoft.AspNetCore.Mvc;

namespace ECOM.Presentation.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
	{
		private readonly IAuthenticationService _authenticationService = authenticationService;

		[HttpPost("signup")]
		public async Task<IActionResult> SignUp([FromBody] BaseRequest<UserSignUp> request)
		{
			var response = await _authenticationService.SignUpAsync(request);
			return Ok(response);
		}

		[HttpPost("signin")]
		public async Task<IActionResult> SignIn([FromBody] BaseRequest<UserSignIn> request)
		{
			var response = await _authenticationService.SignInAsync(request);
			return Ok(response);
		}

		[HttpPost("admin/signin")]
		public async Task<IActionResult> AdminSignIn([FromBody] BaseRequest<UserSignIn> request)
		{
			var response = await _authenticationService.AdminSignInAsync(request);
			return Ok(response);
		}

		[HttpPost("admin/verify-otp")]
		public async Task<IActionResult> AdminVerifyOtp([FromBody] BaseRequest<UserVerifyOtp> request)
		{
			var response = await _authenticationService.AdminVerifyOtpAsync(request);
			return Ok(response);
		}

		[HttpPost("signout")]
		public async Task<IActionResult> SignOut([FromBody] BaseRequest<UserSignOut> request)
		{
			var response = await _authenticationService.SignOutAsync(request);
			return Ok(response);
		}
	}
}
