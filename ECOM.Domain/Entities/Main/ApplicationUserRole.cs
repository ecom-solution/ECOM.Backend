﻿namespace ECOM.Domain.Entities.Main
{
	public class ApplicationUserRole
    {
		public ApplicationUserRole() { }

		public ApplicationUserRole(Guid userId, Guid roleId)
		{
			UserId = userId;
			RoleId = roleId;
		}

		public Guid UserId { get; set; }

		public Guid RoleId { get; set; }

		public virtual ApplicationUser? User { get; set; }
		public virtual ApplicationRole? Role { get; set; }
	}
}
