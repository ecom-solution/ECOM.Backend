using ECOM.App.DTOs.Common;
using ECOM.App.DTOs.Modules.Authentication.Users;

namespace ECOM.App.Services.Interfaces
{
	public interface IAuthenticationService
    {
		/// <summary>
		/// Registers a new user account into the system.
		/// </summary>
		/// <param name="request">The sign-up request containing user registration details.</param>
		/// <returns>A response containing the signed-in user information upon successful registration.</returns>
		Task<BaseResponse<UserSignedIn>> SignUpAsync(BaseRequest<UserSignUp> request);

		/// <summary>
		/// Authenticates a user and generates an access token if credentials are valid.
		/// </summary>
		/// <param name="request">The sign-in request containing user login credentials.</param>
		/// <returns>A response containing the signed-in user information and tokens if authentication is successful.</returns>
		Task<BaseResponse<UserSignedIn>> SignInAsync(BaseRequest<UserSignIn> request);

		/// <summary>
		/// Signs out a user by invalidating their session or refresh tokens.
		/// </summary>
		/// <param name="request">The sign-out request containing user sign-out information.</param>
		/// <returns>A response confirming that the user has successfully signed out.</returns>
		Task<BaseResponse<UserSignedOut>> SignOutAsync(BaseRequest<UserSignedOut> request);
	}
}
