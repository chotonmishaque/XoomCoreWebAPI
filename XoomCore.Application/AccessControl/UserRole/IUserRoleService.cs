using XoomCore.Application.Common.Request;

namespace XoomCore.Application.AccessControl.UserRole;

public interface IUserRoleService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<UserRoleDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<UserRoleDto>> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> CreateAsync(CreateUserRoleRequest postUserRoleRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> UpdateAsync(UpdateUserRoleRequest putUserRoleRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default);

}