using XoomCore.Application.Common.Request;

namespace XoomCore.Application.AccessControl.ActionAuthorization;


public interface IActionAuthorizationService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<ActionAuthorizationDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default);
    Task<CommonResponse<ActionAuthorizationDto>> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> CreateAsync(CreateActionAuthorizationRequest postActionAuthorizationRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> UpdateAsync(UpdateActionAuthorizationRequest putActionAuthorizationRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}