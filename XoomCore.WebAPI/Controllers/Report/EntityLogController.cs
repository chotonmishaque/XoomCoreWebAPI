using XoomCore.Application.Report;

namespace XoomCore.WebAPI.Controllers.Report;

public class EntityLogController : VersionedApiController
{

    private readonly IEntityLogService _entityLogService;
    public EntityLogController(
        IEntityLogService entityLogService
        )
    {
        _entityLogService = entityLogService;
    }

    [HttpGet("search")]
    [MustActionAuthorized(controller: "EntityLog", action: "SearchAsync")]
    [OpenApiOperation("Search EntityLog", "")]
    [ValidateRequest<GetDataTableRequest>()]
    public async Task<CommonDataTableResponse<IEnumerable<EntityLogDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _entityLogService.SearchAsync(getDataTableRequest);
    }
}
