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
        /// Authenticates a admin user and generates an access token if credentials are valid.
        /// </summary>
        /// <param name="request">The sign-in request containing user login credentials.</param>
        /// <returns>A response containing the signed-in admin user information and tokens if authentication is successful.</returns>
        Task<BaseResponse<UserSignedIn>> AdminSignInAsync(BaseRequest<UserSignIn> request);

        /// <summary>
        /// Verifies the OTP (One-Time Password) provided by the admin user during the two-factor authentication (2FA) login process.
        /// </summary>
        /// <param name="request">Request containing username, secret key, and OTP code to verify.</param>
        /// <returns>
        /// Returns a <see cref="BaseResponse{UserSignedIn}"/>:
        /// - Success: If the OTP is valid, generates a new refresh token, access token, and returns user information.
        /// - Failure: If the OTP is invalid, increments failed attempt counters, locks the account if necessary, and returns an error.
        /// </returns>
        /// <remarks>
        /// If the number of failed OTP attempts exceeds the configured limit, the user account will be locked for a predefined period.
        /// </remarks>
        Task<BaseResponse<UserSignedIn>> AdminVerifyOtpAsync(BaseRequest<UserVerifyOtp> request);

        /// <summary>
        /// Signs out a user by invalidating their session or refresh tokens.
        /// </summary>
        /// <param name="request">The sign-out request containing user sign-out information.</param>
        /// <returns>A response confirming that the user has successfully signed out.</returns>
        Task<BaseResponse<UserSignedOut>> SignOutAsync(BaseRequest<UserSignOut> request);
    }
}
