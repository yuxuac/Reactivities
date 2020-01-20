using AutoMapper;
using Domain;
using Application.Activities;
using System.Linq;

namespace Application.Activities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            
            CreateMap<UserActivity, AttendeeDto>()
            .ForMember(dest => dest.Username, o => o.MapFrom(source => source.AppUser.UserName))
            .ForMember(dest => dest.DisplayName, o => o.MapFrom(source => source.AppUser.DisplayName))
            .ForMember(dest => dest.Image, o => o.MapFrom(source => source.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Following, o => o.MapFrom<FollowingResolver>());
        }
    }
}