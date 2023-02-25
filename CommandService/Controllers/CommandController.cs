using AutoMapper;
using CommandService.Data.Entities;
using CommandService.Data.Repos;
using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/platforms/{platformId}/[controller]")]
public class CommandController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommandRepo _commandRepo;

    public CommandController(IMapper mapper, ICommandRepo commandRepo)
    {
        _mapper = mapper;
        _commandRepo = commandRepo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"{nameof(GetCommandsForPlatform)}: {platformId}");

        if (!_commandRepo.IsPlatformExists(platformId))
            return NotFound();

        var commands = _commandRepo.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<CommandReadDto>(commands));
    }


    [HttpGet("{commandId:int}", Name = nameof(GetCommandForPlatform))]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"{nameof(GetCommandForPlatform)}: {platformId} / {commandId}");

        if (!_commandRepo.IsPlatformExists(platformId))
            return NotFound();

        var command = _commandRepo.GetCommand(platformId, commandId);
        if (command == null)
            return NotFound();

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        Console.WriteLine($"{nameof(CreateCommandForPlatform)}: {platformId}");

        if (!_commandRepo.IsPlatformExists(platformId))
            return NotFound();

        var command = _mapper.Map<Command>(commandDto);

        _commandRepo.CreateCommand(platformId, command);
        _commandRepo.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform), new {Id = commandReadDto.Id}, commandReadDto);
    }
}