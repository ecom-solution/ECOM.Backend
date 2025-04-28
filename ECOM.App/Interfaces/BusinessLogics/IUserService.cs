using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.App.Interfaces.BusinessLogics
{
    public interface IUserService
    {
        /// <summary>
        /// Checks if a user has a specific claim.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="claimType">The type of the claim to check.</param>
        /// <param name="claimValue">The value of the claim to check.</param>
        /// <returns>True if the user has the claim, otherwise false.</returns>
        Task<bool> HasClaimAsync(Guid userId, string claimType, string claimValue);

        /// <summary>
        /// Checks if a user belongs to a specific role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleName">The name of the role to check.</param>
        /// <returns>True if the user is in the role, otherwise false.</returns>
        Task<bool> HasRoleAsync(Guid userId, string roleName);

        /// <summary>
        /// Checks if a user has access to the admin area based on their claims.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user has admin access, otherwise false.</returns>
        Task<bool> CanAccessAdminAsync(Guid userId);
    }
}
