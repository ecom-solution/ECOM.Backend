using AutoMapper;
using ECOM.App.DTOs.Modules.Localization;
using ECOM.Domain.Entities.Main;

namespace ECOM.App.Mappings.Profiles
{
	public class LanguageMappingProfile : Profile
	{
		public LanguageMappingProfile()
		{
			CreateMap<LanguageComponent, LanguageComponentVM>().ReverseMap();
			CreateMap<LanguageComponent, LanguageComponentRecordModel>().ReverseMap();
		}
	}
}
