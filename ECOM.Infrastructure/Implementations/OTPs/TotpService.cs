using ECOM.Domain.Interfaces.OTPs;
using ECOM.Shared.Library.Consts;
using OtpNet;
using QRCoder;

namespace ECOM.Infrastructure.Implementations.OTPs
{
	/// <summary>
	/// Default implementation of <see cref="ITotpService"/> using Otp.NET and QRCoder.
	/// Supports TOTP code generation and validation with optional QR code rendering.
	/// </summary>
	public class TotpService : ITotpService
	{
		public string GenerateSecretKey()
		{
			return Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
		}

		public bool VerifyOtp(string otpCode, string secretKey)
		{
			var totp = new Totp(Base32Encoding.ToBytes(secretKey));
			return totp.VerifyTotp(otpCode, out _);
		}

		public string GenerateQrCodeUri(string userName, string secretKey)
		{
			var uri = $"otpauth://totp/{ApplicationConstants.AppName}:{userName}?secret={secretKey}&issuer={ApplicationConstants.AppName}&algorithm=SHA1&digits=6&period=30";
			var qrGenerator = new QRCodeGenerator();
			var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
			var qrCode = new PngByteQRCode(qrCodeData);
			var qrCodeBytes = qrCode.GetGraphic(20);
			return $"data:image/png;base64,{Convert.ToBase64String(qrCodeBytes)}";
		}
	}
}
