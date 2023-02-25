using AutoMapper;
using CommandService.Data.Entities;
using CommandService.DTOs;

namespace CommandService.MapperProfiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        //source --> target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
    }
}