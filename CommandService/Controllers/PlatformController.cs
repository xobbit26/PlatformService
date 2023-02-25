using AutoMapper;
using CommandService.Data.Repos;
using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public PlatformController(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        Console.WriteLine("--> Getting platforms from the CommandService");
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(_commandRepo.GetAllPlatforms()));
    }


    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Service");

        return Ok("Inbound test of from Platform Service");
    }
}