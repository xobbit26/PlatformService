using AutoMapper;
using Grpc.Core;
using PlatformService.Data.Repository;

namespace PlatformService.Services.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IMapper _mapper;
    private readonly IPlatformRepo _platformRepo;

    public GrpcPlatformService(IMapper mapper, IPlatformRepo platformRepo)
    {
        _mapper = mapper;
        _platformRepo = platformRepo;
    }

    public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var response = new PlatformResponse();
        var platforms = await _platformRepo.GetAll();

        foreach (var platform in platforms)
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));

        return response;
    }
}