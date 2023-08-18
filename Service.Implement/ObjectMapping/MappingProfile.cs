using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Newtonsoft.Json.Linq;

namespace Service.Implement.ObjectMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<RefreshToken, RefreshTokenDTO>();
            CreateMap<SampleGreeting, SampleGreetingDTO>();
            CreateMap<TemplateCard, TemplateCardDTO>()
                .ForMember(dest => dest.CategoryIdList, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id).ToList()))
                .ForMember(dest => dest.JsonData, opt =>
                {
                    opt.PreCondition(src => IsValidJson(src.Data!));
                    opt.MapFrom(src => JObject.Parse(src.Data!));
                })
                .ForMember(dest => dest.TagIdList, opt => opt.MapFrom(src => src.Tags.Select(t => t.Id).ToList()));
            CreateMap<UserCard, UserCardDTO>();
            CreateMap<UserInfo, UserInfoDTO>();
            CreateMap<UserUpload, UserUploadDTO>();

            CreateMap<CategoryDTO, Category>();
            CreateMap<RefreshTokenDTO, RefreshToken>();
            CreateMap<SampleGreetingDTO, SampleGreeting>();
            CreateMap<TemplateCardDTO, TemplateCard>();
            CreateMap<UserCardDTO, UserCard>();
            CreateMap<UserInfoDTO, UserInfo>();
            CreateMap<UserUploadDTO, UserUpload>();
        }

        private bool IsValidJson(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue)) { return false; }
            try
            {
                var obj = JObject.Parse(stringValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
