using XoomCore.Application.AccessControl.Role;
using XoomCore.Application.Common.Request;

namespace XoomCore.Services.Concretes.AccessControl;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _iMapper = mapper;
    }
    public async Task<CommonDataTableResponse<IEnumerable<RoleDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getRoleListQuery = _unitOfWork.RoleRepository
                    .GetAll(cancellationToken)
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        if (await getRoleListQuery.LongCountAsync() is long totalRowCount && totalRowCount < 1)
        {
            return CommonDataTableResponse<IEnumerable<RoleDto>>.CreateWarning();
        }
        List<Role> responseRoleList = await getRoleListQuery
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync(cancellationToken);
        IEnumerable<RoleDto> mappedRoleList = _iMapper.Map<List<RoleDto>>(responseRoleList);

        return CommonDataTableResponse<IEnumerable<RoleDto>>.CreateSuccess(totalRowCount, mappedRoleList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default)
    {
        List<Role> respRoleList = await _unitOfWork.RoleRepository
                                .GetAll(cancellationToken)
                                .Where(x => x.Status == EntityStatus.IsActive)
                                .ToListAsync(cancellationToken);
        if (!respRoleList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarning();
        }

        IEnumerable<SelectOptionResponse> mappedRoleCategories = _iMapper.Map<List<SelectOptionResponse>>(respRoleList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateSuccess(mappedRoleCategories.OrderByDescending(x => x.Id));
    }

    public async Task<CommonResponse<RoleDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var respRole = await _unitOfWork.RoleRepository.GetAsync(id, cancellationToken);
        if (respRole == null)
        {
            return CommonResponse<RoleDto>.CreateWarning();
        }

        RoleDto mappedRole = _iMapper.Map<RoleDto>(respRole);
        return CommonResponse<RoleDto>.CreateSuccess(mappedRole);
    }

    public async Task<CommonResponse<long>> CreateAsync(CreateRoleRequest createRoleRequest, CancellationToken cancellationToken = default)
    {
        Role mappedRole = _iMapper.Map<Role>(createRoleRequest);
        //_sessionManager.SetInsertedIdentity(mappedRole);

        await _unitOfWork.RoleRepository.AddAsync(mappedRole, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(mappedRole.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> UpdateAsync(UpdateRoleRequest updateRoleRequest, CancellationToken cancellationToken = default)
    {
        Role existRole = await _unitOfWork.RoleRepository.GetAsync(updateRoleRequest.Id, cancellationToken);
        if (existRole == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        Role oldValue = existRole;
        Role newValue = _iMapper.Map(updateRoleRequest, existRole);

        //_sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.RoleRepository.UpdateAsync(oldValue, newValue, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existRole.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        Role existRole = await _unitOfWork.RoleRepository.GetAsync(id, cancellationToken);
        if (existRole == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }

        await _unitOfWork.RoleRepository.DeleteAsync(existRole, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existRole.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }
}
