using AutoMapper;
using ECOM.Domain.Entities.Main;
using ECOM.App.DTOs.Modules.Authentication.Roles;
using ECOM.App.DTOs.Modules.Authentication.Claims;

namespace ECOM.App.Mappings.Profiles
{
	public class AuthenticationMappingProfile : Profile
	{
		public AuthenticationMappingProfile()
		{
			CreateMap<ApplicationRole, RoleVM>().ReverseMap();
			CreateMap<ApplicationClaim, ClaimVM>().ReverseMap();
		}
	}
}
