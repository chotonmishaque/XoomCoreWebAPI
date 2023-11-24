using XoomCore.Application.AccessControl.SubMenu;
using XoomCore.Application.Common.Request;

namespace XoomCore.Services.Concretes.AccessControl;

public class SubMenuService : ISubMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;
    public SubMenuService(IUnitOfWork unitOfWork, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<SubMenuDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getSubMenuListQuery = _unitOfWork.SubMenuRepository
                    .GetAll(cancellationToken)
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));
        if (await getSubMenuListQuery.LongCountAsync() is long totalRowCount && totalRowCount < 1)
        {
            return CommonDataTableResponse<IEnumerable<SubMenuDto>>.CreateWarning();
        }

        List<SubMenu> responseSubMenuList = await getSubMenuListQuery
                                    .OrderByDescending(x => x.Id)
                                    .Include("Menu")
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();

        IEnumerable<SubMenuDto> mappedSubMenuList = _iMapper.Map<List<SubMenuDto>>(responseSubMenuList);

        return CommonDataTableResponse<IEnumerable<SubMenuDto>>.CreateSuccess(totalRowCount, responseSubMenuList.LongCount(), mappedSubMenuList);
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default)
    {
        List<SubMenu> responseSubMenuList = await _unitOfWork.SubMenuRepository.GetAll(cancellationToken).ToListAsync();
        if (!responseSubMenuList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarning();
        }

        IEnumerable<SelectOptionResponse> mappedSubMenuList = _iMapper.Map<List<SelectOptionResponse>>(responseSubMenuList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateSuccess(mappedSubMenuList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<SubMenuDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        SubMenu responseSubMenu = await _unitOfWork.SubMenuRepository.Get(x => x.Id == id)
                                    .Include("Menu")
                                    .SingleOrDefaultAsync(cancellationToken);
        if (responseSubMenu == null)
        {
            return CommonResponse<SubMenuDto>.CreateWarning();
        }

        SubMenuDto mappedSubMenu = _iMapper.Map<SubMenuDto>(responseSubMenu);
        return CommonResponse<SubMenuDto>.CreateSuccess(mappedSubMenu);
    }

    public async Task<CommonResponse<long>> CreateAsync(CreateSubMenuRequest createSubMenuRequest, CancellationToken cancellationToken = default)
    {
        SubMenu mappedSubMenu = _iMapper.Map<SubMenu>(createSubMenuRequest);
        //_sessionManager.SetInsertedIdentity(mappedSubMenu);

        await _unitOfWork.SubMenuRepository.AddAsync(mappedSubMenu, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(mappedSubMenu.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> UpdateAsync(UpdateSubMenuRequest updateSubMenuRequest, CancellationToken cancellationToken = default)
    {
        SubMenu existSubMenu = await _unitOfWork.SubMenuRepository.GetAsync(updateSubMenuRequest.Id, cancellationToken);
        if (existSubMenu == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        SubMenu oldValue = existSubMenu;
        SubMenu newValue = _iMapper.Map(updateSubMenuRequest, existSubMenu);
        //updateSubMenuRequest.MapTo(existSubMenu);

        //_sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.SubMenuRepository.UpdateAsync(oldValue, newValue, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existSubMenu.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        SubMenu existSubMenu = await _unitOfWork.SubMenuRepository.GetAsync(id, cancellationToken);
        if (existSubMenu == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        //_sessionManager.SetDeletedIdentity(existSubMenu);
        await _unitOfWork.SubMenuRepository.DeleteAsync(existSubMenu, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existSubMenu.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }
}