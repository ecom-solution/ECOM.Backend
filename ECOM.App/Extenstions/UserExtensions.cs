using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;

namespace ECOM.App.Extenstions
{
	public static class UserExtensions
	{
		public static async Task<bool> HasRoleAsync(this IRepository<ApplicationUserRole> repo, Guid userId, string roleName)
		{
			var query = repo.Include(ur => ur.Role)
				.Where(ur => ur.UserId == userId &&
							 ur.Role != null &&
							 ur.Role.Name == roleName);

			return await repo.AnyAsync(query);
		}
	}
}
