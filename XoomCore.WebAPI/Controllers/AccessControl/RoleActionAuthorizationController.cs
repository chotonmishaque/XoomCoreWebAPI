using XoomCore.Application.AccessControl.RoleActionAuthorization;
using XoomCore.WebAPI.Controllers;

namespace XoomCore.Web.AccessControl.Controllers;

public class RoleActionAuthorizationController : VersionedApiController
{

    private readonly IRoleActionAuthorizationService _roleActionAuthorizationService;
    public RoleActionAuthorizationController(
        IRoleActionAuthorizationService roleActionAuthorizationService
        )
    {
        _roleActionAuthorizationService = roleActionAuthorizationService;
    }

    /*****************************
        
        RoleActionAuthorization Related Action Start

    *****************************/

    [HttpGet("search")]
    [MustActionAuthorized(controller: "RoleActionAuthorization", action: "SearchAsync")]
    [OpenApiOperation("Search RoleActionAuthorization", "")]
    [ValidateRequest<GetDataTableRequest>()]
    public async Task<CommonDataTableResponse<IEnumerable<RoleActionAuthorizationDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken)
    {
        return await _roleActionAuthorizationService.SearchAsync(getDataTableRequest, cancellationToken);
    }

    [HttpGet("{id:long}")]
    [MustActionAuthorized(controller: "RoleActionAuthorization", action: "GetAsync")]
    [OpenApiOperation("Get RoleActionAuthorization", "")]
    public async Task<CommonResponse<RoleActionAuthorizationDto>> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await _roleActionAuthorizationService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    [MustActionAuthorized(controller: "RoleActionAuthorization", action: "CreateAsync")]
    [OpenApiOperation("Create Async", "")]
    [ValidateRequest<CreateRoleActionAuthorizationRequest>()]
    public async Task<CommonResponse<long>> CreateAsync([FromBody] CreateRoleActionAuthorizationRequest postRoleActionAuthorizationRequest, CancellationToken cancellationToken)
    {
        return await _roleActionAuthorizationService.CreateAsync(postRoleActionAuthorizationRequest, cancellationToken);
    }

    [HttpPut]
    [MustActionAuthorized(controller: "RoleActionAuthorization", action: "UpdateAsync")]
    [OpenApiOperation("Update Async", "")]
    [ValidateRequest<UpdateRoleActionAuthorizationRequest>()]
    public async Task<CommonResponse<long>> UpdateAsync([FromBody] UpdateRoleActionAuthorizationRequest putRoleActionAuthorizationRequest, CancellationToken cancellationToken)
    {
        return await _roleActionAuthorizationService.UpdateAsync(putRoleActionAuthorizationRequest, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    [MustActionAuthorized(controller: "RoleActionAuthorization", action: "DeleteAsync")]
    [OpenApiOperation("Delete Async", "")]
    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        return await _roleActionAuthorizationService.DeleteAsync(id, cancellationToken);
    }

    /*****************************

       RoleActionAuthorization Related Action End

    *****************************/
}
