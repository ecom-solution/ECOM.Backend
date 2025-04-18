namespace ECOM.Shared.Library.Enums.Entity
{
	/// <summary>
	/// Represents the status of a user account in the system.
	/// </summary>
	public enum UserStatusEnum : int
	{
		/// <summary>
		/// The account is newly created and may not be verified yet.
		/// </summary>
		New = 0,

		/// <summary>
		/// The account is active and has full access to the system.
		/// </summary>
		Active = 1,

		/// <summary>
		/// The account is inactive, possibly locked, disabled, or deactivated.
		/// </summary>
		Inactive = 2
	}

	/// <summary>
	/// Represents the real-time connection status of a user.
	/// </summary>
	public enum ConnectionStatusEnum : int
	{
		/// <summary>
		/// The user is currently connected and active (e.g., online via SignalR).
		/// </summary>
		Online = 0,

		/// <summary>
		/// The user is not currently connected (disconnected, logged out, or idle).
		/// </summary>
		Offline = 1
	}
}
