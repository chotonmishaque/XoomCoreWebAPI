using XoomCore.Application.Common.Request;

namespace XoomCore.Application.AccessControl.RoleActionAuthorization;

public interface IRoleActionAuthorizationService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<RoleActionAuthorizationDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default);
    Task<CommonResponse<RoleActionAuthorizationDto>> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> CreateAsync(CreateRoleActionAuthorizationRequest createRoleActionAuthorizationRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> UpdateAsync(UpdateRoleActionAuthorizationRequest updateRoleActionAuthorizationRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}