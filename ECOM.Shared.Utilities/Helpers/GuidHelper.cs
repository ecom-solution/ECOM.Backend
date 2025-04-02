using System.Security.Cryptography;

namespace ECOM.Shared.Utilities.Helpers
{
	public static class GuidHelper
	{
		public static Guid GenerateSequenceGuid()
		{
			var randomBytes = new byte[10];
			RandomNumberGenerator.Fill(randomBytes);

			var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

			var guidBytes = new byte[16];
			Buffer.BlockCopy(timestamp, 2, guidBytes, 0, 6);
			Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

			return new Guid(guidBytes);
		}
	}
}
