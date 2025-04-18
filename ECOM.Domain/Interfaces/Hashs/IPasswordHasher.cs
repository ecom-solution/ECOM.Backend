namespace ECOM.Domain.Interfaces.Hashs
{
	/// <summary>
	/// Provides methods to hash and verify passwords.
	/// </summary>
	public interface IPasswordHasher
	{
		/// <summary>
		/// Hashes the given plain text password.
		/// </summary>
		/// <param name="password">The plain password to hash.</param>
		/// <returns>A hashed string using a secure algorithm.</returns>
		string Hash(string password);

		/// <summary>
		/// Verifies whether a plain password matches a hashed value.
		/// </summary>
		/// <param name="password">The input plain password.</param>
		/// <param name="hashedPassword">The stored hashed password.</param>
		/// <returns>True if they match; otherwise, false.</returns>
		bool Verify(string password, string hashedPassword);
	}
}
