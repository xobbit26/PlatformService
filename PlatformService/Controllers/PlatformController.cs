using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Entities;
using PlatformService.Data.Repository;
using PlatformService.DTOs;
using PlatformService.Services.SyncDataServices;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformController(IPlatformRepo platformRepo,
        IMapper mapper,
        ICommandDataClient commandDataClient)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public async Task<ActionResult<List<IEnumerable<Platform>>>> GetAll()
    {
        var platforms = await _platformRepo.GetAll();
        return Ok(platforms.Select(p => _mapper.Map<PlatformReadDto>(p)));
    }

    //TODO: Check the response as action result. Are there any differences
    [HttpGet("{id:int}", Name = nameof(GetById))]
    public async Task<ActionResult<PlatformReadDto>> GetById(int id)
    {
        var platform = await _platformRepo.GetById(id);

        if (platform != null)
            return Ok(_mapper.Map<PlatformReadDto>(platform));

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Create(PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);
        await _platformRepo.Create(platform);

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not sent synchronously: {ex.Message}");
        }


        return CreatedAtRoute(nameof(GetById), new {Id = platformReadDto.Id}, platformReadDto);
    }
}