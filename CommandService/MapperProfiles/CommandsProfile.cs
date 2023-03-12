using AutoMapper;
using CommandService.Data.Entities;
using CommandService.DTOs;
using CommandService.Events;

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
    }
}