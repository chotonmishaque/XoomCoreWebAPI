using XoomCore.Application.Common.Request;

namespace XoomCore.Application.AccessControl.SubMenu;

public interface ISubMenuService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<SubMenuDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default);
    Task<CommonResponse<SubMenuDto>> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> CreateAsync(CreateSubMenuRequest createSubMenuRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> UpdateAsync(UpdateSubMenuRequest updateSubMenuRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}