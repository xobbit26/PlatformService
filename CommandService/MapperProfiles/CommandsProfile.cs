using AutoMapper;
using CommandService.Data.Entities;
using CommandService.DTOs;
using CommandService.Events;
using PlatformService;

namespace CommandService.MapperProfiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        //source --> target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();

        CreateMap<PlatformPublishedEvent, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));


        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
}