namespace XoomCore.WebAPI.Controllers.AccessControl;

public class ActionAuthorizationController : VersionedApiController
{

    private readonly IActionAuthorizationService _actionAuthorizationService;
    public ActionAuthorizationController(
        IActionAuthorizationService ActionAuthorizationService
        )
    {
        _actionAuthorizationService = ActionAuthorizationService;
    }

    /*****************************
        
        ActionAuthorization Related Action Start

    *****************************/

    [HttpGet("search")]
    [MustActionAuthorized(controller: "ActionAuthorization", action: "SearchAsync")]
    [OpenApiOperation("Search ActionAuthorization", "")]
    [ValidateRequest<GetDataTableRequest>()]
    public async Task<CommonDataTableResponse<IEnumerable<ActionAuthorizationDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken)
    {
        return await _actionAuthorizationService.SearchAsync(getDataTableRequest, cancellationToken);
    }

    [HttpGet("{id:long}")]
    [MustActionAuthorized(controller: "ActionAuthorization", action: "GetAsync")]
    [OpenApiOperation("Get ActionAuthorization", "")]
    public async Task<CommonResponse<ActionAuthorizationDto>> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await _actionAuthorizationService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    [MustActionAuthorized(controller: "ActionAuthorization", action: "CreateAsync")]
    [OpenApiOperation("Create Async", "")]
    [ValidateRequest<CreateActionAuthorizationRequest>()]
    public async Task<CommonResponse<long>> CreateAsync([FromBody] CreateActionAuthorizationRequest createActionAuthorizationRequest, CancellationToken cancellationToken)
    {
        return await _actionAuthorizationService.CreateAsync(createActionAuthorizationRequest, cancellationToken);
    }

    [HttpPut]
    [MustActionAuthorized(controller: "ActionAuthorization", action: "UpdateAsync")]
    [OpenApiOperation("Update Async", "")]
    [ValidateRequest<UpdateActionAuthorizationRequest>()]
    public async Task<CommonResponse<long>> UpdateAsync([FromBody] UpdateActionAuthorizationRequest updateActionAuthorizationRequest, CancellationToken cancellationToken)
    {
        return await _actionAuthorizationService.UpdateAsync(updateActionAuthorizationRequest, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    [MustActionAuthorized(controller: "ActionAuthorization", action: "DeleteAsync")]
    [OpenApiOperation("Delete Async", "")]
    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        return await _actionAuthorizationService.DeleteAsync(id, cancellationToken);
    }

    /*****************************

       ActionAuthorization Related Action End

    *****************************/
}
