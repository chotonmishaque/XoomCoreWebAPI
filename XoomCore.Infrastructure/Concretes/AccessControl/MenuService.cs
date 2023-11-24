using XoomCore.Application.AccessControl.Menu;
using XoomCore.Application.Common.Request;

namespace XoomCore.Services.Concretes.AccessControl;

public class MenuService : IMenuService
{
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;
    public MenuService(ICurrentUser currentUser, IUnitOfWork unitOfWork, IMapper iMapper)
    {
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<MenuDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getMenuListQuery = _unitOfWork.MenuRepository
                    .GetAll(cancellationToken)
                    .OrderByDescending(x => x.Id)
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        if (await getMenuListQuery.LongCountAsync() is long totalRowCount && totalRowCount < 1)
        {
            return CommonDataTableResponse<IEnumerable<MenuDto>>.CreateWarning();
        }

        List<Menu> responseMenuList = await getMenuListQuery
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();

        IEnumerable<MenuDto> mappedMenuList = _iMapper.Map<List<MenuDto>>(responseMenuList);

        return CommonDataTableResponse<IEnumerable<MenuDto>>.CreateSuccess(totalRowCount, mappedMenuList.LongCount(), mappedMenuList);
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default)
    {
        List<Menu> responseMenuList = await _unitOfWork.MenuRepository.GetAll(cancellationToken).ToListAsync();
        if (!responseMenuList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarning();
        }

        IEnumerable<SelectOptionResponse> mappedMenuList = _iMapper.Map<List<SelectOptionResponse>>(responseMenuList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateSuccess(mappedMenuList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<MenuDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        Menu responseMenu = await _unitOfWork.MenuRepository.GetAsync(id, cancellationToken);
        if (responseMenu == null)
        {
            return CommonResponse<MenuDto>.CreateWarning();
        }

        MenuDto mappedMenu = _iMapper.Map<MenuDto>(responseMenu);
        return CommonResponse<MenuDto>.CreateSuccess(mappedMenu);
    }

    public async Task<CommonResponse<long>> CreateAsync(CreateMenuRequest createMenuRequest, CancellationToken cancellationToken = default)
    {
        Menu mappedMenu = _iMapper.Map<Menu>(createMenuRequest);
        _currentUser.SetInsertedIdentity(mappedMenu);
        mappedMenu.CreatedBy = _currentUser.GetUserId();

        await _unitOfWork.MenuRepository.AddAsync(mappedMenu, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(mappedMenu.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> UpdateAsync(UpdateMenuRequest updateMenuRequest, CancellationToken cancellationToken = default)
    {
        Menu existMenu = await _unitOfWork.MenuRepository.GetAsync(updateMenuRequest.Id, cancellationToken);
        if (existMenu == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        Menu oldValue = existMenu;
        Menu newValue = _iMapper.Map(updateMenuRequest, existMenu);
        //updateMenuRequest.MapTo(existMenu);
        _currentUser.SetUpdatedIdentity(newValue);

        await _unitOfWork.MenuRepository.UpdateAsync(oldValue, newValue, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existMenu.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        Menu existMenu = await _unitOfWork.MenuRepository.GetAsync(id, cancellationToken);
        if (existMenu == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }

        await _unitOfWork.MenuRepository.DeleteAsync(existMenu, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existMenu.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }
}