using XoomCore.Application.Common.Request;

namespace XoomCore.Application.AccessControl.Role;

public interface IRoleService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<RoleDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default);
    Task<CommonResponse<RoleDto>> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> CreateAsync(CreateRoleRequest createRoleRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> UpdateAsync(UpdateRoleRequest updateRoleRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
