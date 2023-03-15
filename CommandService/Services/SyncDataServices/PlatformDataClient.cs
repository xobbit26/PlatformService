using AutoMapper;
using CommandService.Data.Entities;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.Services.SyncDataServices;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly ILogger<PlatformDataClient> _logger;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public PlatformDataClient(ILogger<PlatformDataClient> logger, IMapper mapper, IConfiguration config)
    {
        _logger = logger;
        _mapper = mapper;
        _config = config;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        _logger.LogInformation($"Calling GRPC Service: {_config["GrpcPlatformUrl"]}");

        //todo: improve this part and check for null
        var channel = GrpcChannel.ForAddress(_config["GrpcPlatformUrl"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not call GRPC Server");
            return null;
        }
    }
}