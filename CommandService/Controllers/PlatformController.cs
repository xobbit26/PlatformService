using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("c/[controller]")]
public class PlatformController : ControllerBase
{
    [HttpPost]
    public void CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        Console.WriteLine("--> CreatePlatform has been invoked");
    }
}