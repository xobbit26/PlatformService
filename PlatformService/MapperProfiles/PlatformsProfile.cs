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
    }
}