using AutoMapper;
using PlatformService.Data.Entities;
using PlatformService.DTOs;
using PlatformService.Events;

namespace PlatformService.MapperProfiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        //Source -> Target
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<Platform, PlatformReadDto>();

        CreateMap<PlatformReadDto, PlatformPublishedEvent>();

        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dest => dest.PlatformId, otp => otp.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, otp => otp.MapFrom(src => src.Name))
            .ForMember(dest => dest.Publisher, otp => otp.MapFrom(src => src.Publisher));
    }
}