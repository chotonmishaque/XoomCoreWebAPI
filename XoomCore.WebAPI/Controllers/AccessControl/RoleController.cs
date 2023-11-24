using XoomCore.Application.AccessControl.Role;

namespace XoomCore.WebAPI.Controllers.AccessControl;

public class RoleController : VersionedApiController
{
    private readonly IRoleService _roleService;
    public RoleController(
        IRoleService RoleService
        )
    {
        _roleService = RoleService;
    }

    /*****************************
        
        Role Related Action Start

    *****************************/

    [HttpGet("search")]
    [MustActionAuthorized(controller: "Role", action: "SearchAsync")]
    [OpenApiOperation("Search Role", "")]
    [ValidateRequest<GetDataTableRequest>()]
    public async Task<CommonDataTableResponse<IEnumerable<RoleDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        return await _roleService.SearchAsync(getDataTableRequest, cancellationToken);
    }

    [HttpGet("{id:long}")]
    [MustActionAuthorized(controller: "Role", action: "GetAsync")]
    [OpenApiOperation("Get Role", "")]
    public async Task<CommonResponse<RoleDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _roleService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    [MustActionAuthorized(controller: "Role", action: "CreateAsync")]
    [OpenApiOperation("Create Async", "")]
    [ValidateRequest<CreateRoleRequest>()]
    public async Task<CommonResponse<long>> CreateAsync([FromBody] CreateRoleRequest createRoleRequest, CancellationToken cancellationToken = default)
    {
        return await _roleService.CreateAsync(createRoleRequest, cancellationToken);
    }

    [HttpPut]
    [MustActionAuthorized(controller: "Role", action: "UpdateAsync")]
    [OpenApiOperation("Update Async", "")]
    [ValidateRequest<UpdateRoleRequest>()]
    public async Task<CommonResponse<long>> UpdateAsync([FromBody] UpdateRoleRequest updateRoleRequest, CancellationToken cancellationToken = default)
    {
        return await _roleService.UpdateAsync(updateRoleRequest, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    [MustActionAuthorized(controller: "Role", action: "DeleteAsync")]
    [OpenApiOperation("Delete Async", "")]
    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _roleService.DeleteAsync(id, cancellationToken);
    }

    /*****************************

       Role Related Action End

    *****************************/
}
