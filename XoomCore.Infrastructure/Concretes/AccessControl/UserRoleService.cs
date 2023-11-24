using XoomCore.Application.AccessControl.UserRole;
using XoomCore.Application.Common.Request;

namespace XoomCore.Services.Concretes.AccessControl;

public class UserRoleService : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;
    public UserRoleService(IUnitOfWork unitOfWork, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<UserRoleDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getUserRoleListQuery = _unitOfWork.UserRoleRepository
                    .GetAll()
                    .Where(x => x.Role.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getUserRoleListQuery.CountAsync();

        List<UserRole> responseUserRoleList = await getUserRoleListQuery
                                    .Include(x => x.Role)
                                    .Include(x => x.User)
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseUserRoleList.Any())
        {
            return CommonDataTableResponse<IEnumerable<UserRoleDto>>.CreateWarning();
        }

        IEnumerable<UserRoleDto> mappedUserRoleList = _iMapper.Map<List<UserRoleDto>>(responseUserRoleList);

        return CommonDataTableResponse<IEnumerable<UserRoleDto>>.CreateSuccess(totalRowCount, mappedUserRoleList.OrderByDescending(x => x.Id));
    }

    public async Task<CommonResponse<UserRoleDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        UserRole responseUserRole = await _unitOfWork.UserRoleRepository.GetAsync(id, cancellationToken);
        if (responseUserRole == null)
        {
            return CommonResponse<UserRoleDto>.CreateWarning();
        }

        UserRoleDto mappedUserRole = _iMapper.Map<UserRoleDto>(responseUserRole);
        return CommonResponse<UserRoleDto>.CreateSuccess(mappedUserRole);
    }

    public async Task<CommonResponse<long>> CreateAsync(CreateUserRoleRequest createUserRoleRequest, CancellationToken cancellationToken = default)
    {
        UserRole mappedUserRole = _iMapper.Map<UserRole>(createUserRoleRequest);
        //_sessionManager.SetInsertedIdentity(mappedUserRole);

        await _unitOfWork.UserRoleRepository.AddAsync(mappedUserRole, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(mappedUserRole.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> UpdateAsync(UpdateUserRoleRequest updateUserRoleRequest, CancellationToken cancellationToken = default)
    {
        UserRole existUserRole = await _unitOfWork.UserRoleRepository.GetAsync(updateUserRoleRequest.Id, cancellationToken);
        if (existUserRole == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        UserRole oldValue = existUserRole;
        UserRole newValue = _iMapper.Map(updateUserRoleRequest, existUserRole);
        //updateUserRoleRequest.MapTo(existUserRole);

        //_sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.UserRoleRepository.UpdateAsync(oldValue, newValue, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existUserRole.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        UserRole existUserRole = await _unitOfWork.UserRoleRepository.GetAsync(id, cancellationToken);
        if (existUserRole == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }

        await _unitOfWork.UserRoleRepository.DeleteAsync(existUserRole, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existUserRole.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }
}