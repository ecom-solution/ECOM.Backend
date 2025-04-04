using AutoMapper;
using ECOM.Domain.Entities.Main;
using ECOM.App.DTOs.Modules.Authentication.Users;
using ECOM.App.DTOs.Modules.Authentication.Roles;
using ECOM.App.DTOs.Modules.Authentication.Claims;

namespace ECOM.App.Mappings.Profiles
{
	public class AuthenticationMappingProfile : Profile
	{
		public AuthenticationMappingProfile()
		{
			CreateMap<ApplicationUser, UserVM>().ReverseMap();
			CreateMap<ApplicationRole, RoleVM>().ReverseMap();
			CreateMap<ApplicationClaim, ClaimVM>().ReverseMap();
		}
	}
}
