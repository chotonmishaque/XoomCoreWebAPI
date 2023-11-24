using XoomCore.Application.AccessControl.UserRole;
using XoomCore.WebAPI.Controllers;

namespace XoomCore.Web.AccessControl.Controllers;

public class UserRoleController : VersionedApiController
{

    private readonly IUserRoleService _UserRoleService;
    public UserRoleController(
        IUserRoleService UserRoleService
        )
    {
        _UserRoleService = UserRoleService;
    }

    /*****************************
        
        UserRole Related Action Start

    *****************************/

    [HttpGet("search")]
    [MustActionAuthorized(controller: "UserRole", action: "SearchAsync")]
    [OpenApiOperation("Search UserRole", "")]
    [ValidateRequest<GetDataTableRequest>()]
    public async Task<CommonDataTableResponse<IEnumerable<UserRoleDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        return await _UserRoleService.SearchAsync(getDataTableRequest, cancellationToken);
    }

    [HttpGet("{id:long}")]
    [MustActionAuthorized(controller: "UserRole", action: "GetAsync")]
    [OpenApiOperation("Get UserRole", "")]
    public async Task<CommonResponse<UserRoleDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _UserRoleService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    [MustActionAuthorized(controller: "UserRole", action: "CreateAsync")]
    [OpenApiOperation("Create Async", "")]
    [ValidateRequest<CreateUserRoleRequest>()]
    public async Task<CommonResponse<long>> CreateAsync([FromBody] CreateUserRoleRequest postUserRoleRequest, CancellationToken cancellationToken = default)
    {
        return await _UserRoleService.CreateAsync(postUserRoleRequest, cancellationToken);
    }

    [HttpPut]
    [MustActionAuthorized(controller: "UserRole", action: "UpdateAsync")]
    [OpenApiOperation("Update Async", "")]
    [ValidateRequest<UpdateUserRoleRequest>()]
    public async Task<CommonResponse<long>> UpdateAsync([FromBody] UpdateUserRoleRequest putUserRoleRequest, CancellationToken cancellationToken = default)
    {
        return await _UserRoleService.UpdateAsync(putUserRoleRequest, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    [MustActionAuthorized(controller: "UserRole", action: "DeleteAsync")]
    [OpenApiOperation("Delete Async", "")]
    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _UserRoleService.DeleteAsync(id, cancellationToken);
    }

    /*****************************

       UserRole Related Action End

    *****************************/
}
