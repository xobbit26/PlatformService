using AutoMapper;
using CommandService.Data.Repos;
using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly ILogger<CommandController> _logger;
    private readonly IMapper _mapper;
    private readonly ICommandRepo _commandRepo;

    public PlatformController(
        ILogger<CommandController> logger,
        IMapper mapper,
        ICommandRepo commandRepo
    )
    {
        _logger = logger;
        _mapper = mapper;
        _commandRepo = commandRepo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        _logger.LogInformation("--> Getting platforms from the CommandService");
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(_commandRepo.GetAllPlatforms()));
    }


    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        _logger.LogInformation(("--> Inbound POST # Command Service"));
        return Ok("Inbound test of from Platform Service");
    }
}