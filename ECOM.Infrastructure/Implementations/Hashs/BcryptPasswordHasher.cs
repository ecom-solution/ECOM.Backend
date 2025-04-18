using ECOM.Domain.Interfaces.Hashs;

namespace ECOM.Infrastructure.Implementations.Hashs
{
	/// <summary>
	/// BCrypt-based implementation of <see cref="IPasswordHasher"/>.
	/// Uses BCrypt.Net for password hashing and verification.
	/// </summary>
	public class BcryptPasswordHasher : IPasswordHasher
	{
		public string Hash(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		public bool Verify(string password, string hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}
	}
}
