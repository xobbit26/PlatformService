using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Entities;
using PlatformService.Data.Repository;
using PlatformService.DTOs;
using PlatformService.Events;
using PlatformService.Services.AsyncDataServices;
using PlatformService.Services.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformController : ControllerBase
{
    //TODO: add globalExceptionHandler
    //TODO: create services layer

    private readonly ILogger<PlatformController> _logger;
    private readonly IMapper _mapper;
    private readonly IPlatformRepo _platformRepo;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IEventBus _eventBus;

    public PlatformController(
        ILogger<PlatformController> logger,
        IMapper mapper,
        IPlatformRepo platformRepo,
        ICommandDataClient commandDataClient,
        IEventBus eventBus)
    {
        _logger = logger;
        _mapper = mapper;
        _platformRepo = platformRepo;
        _commandDataClient = commandDataClient;
        _eventBus = eventBus;
    }

    [HttpGet]
    public async Task<ActionResult<List<IEnumerable<Platform>>>> GetAll()
    {
        var platforms = await _platformRepo.GetAll();
        return Ok(platforms.Select(p => _mapper.Map<PlatformReadDto>(p)));
    }

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


        //send synchronously
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not sent synchronously: {ex.Message}");
        }

        //send asynchronously
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedEvent>(platformReadDto);
            _eventBus.SendMessage(platformPublishedDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Could not sent asynchronously: {ex.Message}");
        }


        return CreatedAtRoute(nameof(GetById), new {platformReadDto.Id}, platformReadDto);
    }
}