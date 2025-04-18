namespace ECOM.Domain.Interfaces.OTPs
{
	/// <summary>
	/// Provides functionalities for generating and validating TOTP (Time-based One-Time Password) codes.
	/// Also includes QR code URI generation for authenticator apps.
	/// </summary>
	public interface ITotpService
	{
		/// <summary>
		/// Generates a new random secret key encoded in Base32.
		/// </summary>
		/// <returns>A new Base32-encoded secret key.</returns>
		string GenerateSecretKey();

		/// <summary>
		/// Verifies an OTP code based on the shared secret key.
		/// </summary>
		/// <param name="secretKey">The shared secret key in Base32 format.</param>
		/// <param name="otpCode">The OTP code to verify.</param>
		/// <returns>True if valid, otherwise false.</returns>
		bool VerifyOtp(string secretKey, string otpCode);

		/// <summary>
		/// Generates a URI compatible with authenticator apps to scan and set up TOTP.
		/// </summary>
		/// <param name="userName">The username associated with the OTP.</param>
		/// <param name="secretKey">The Base32-encoded secret key.</param>
		/// <returns>A URI string formatted for QR code generation.</returns>
		string GenerateQrCodeUri(string userName, string secretKey);
	}
}
