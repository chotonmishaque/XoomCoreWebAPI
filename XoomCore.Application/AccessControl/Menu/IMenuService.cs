using XoomCore.Application.Common.Request;

namespace XoomCore.Application.AccessControl.Menu;

public interface IMenuService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<MenuDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default);
    Task<CommonResponse<MenuDto>> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> CreateAsync(CreateMenuRequest createMenuRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> UpdateAsync(UpdateMenuRequest updateMenuRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default);

}
