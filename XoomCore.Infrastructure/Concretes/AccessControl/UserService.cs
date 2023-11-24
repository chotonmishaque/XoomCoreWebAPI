using XoomCore.Application.AccessControl.User;
using XoomCore.Application.Common.Request;
using XoomCore.Infrastructure.Caching;
using XoomCore.Infrastructure.Helpers;

namespace XoomCore.Services.Concretes.AccessControl;

public class UserService : IUserService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICacheManager _cacheManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;
    public UserService(
        ICurrentUser currentUser,
        ICacheManager cacheManager,
        IUnitOfWork unitOfWork,
        IMapper iMapper)
    {
        _currentUser = currentUser;
        _cacheManager = cacheManager;
        _unitOfWork = unitOfWork;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<UserDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getUserListQuery = _unitOfWork.UserRepository
                    .GetAll()
                    .Where(x => x.Username.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        if (await getUserListQuery.LongCountAsync() is long totalRowCount && totalRowCount < 1)
        {
            return CommonDataTableResponse<IEnumerable<UserDto>>.CreateWarning();
        }

        List<User> responseUserList = await getUserListQuery
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();

        IEnumerable<UserDto> mappedUserList = _iMapper.Map<List<UserDto>>(responseUserList);

        return CommonDataTableResponse<IEnumerable<UserDto>>.CreateSuccess(totalRowCount, mappedUserList.OrderByDescending(x => x.Id));
    }

    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default)
    {
        List<User> responseUserList = await _unitOfWork.UserRepository
                                    .GetAll()
                                    .Where(x => x.Status == UserStatus.IsActive)
                                    .ToListAsync();
        if (!responseUserList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarning();
        }

        IEnumerable<SelectOptionResponse> mappedUserList = _iMapper.Map<List<SelectOptionResponse>>(responseUserList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateSuccess(mappedUserList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<UserDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        User responseUser = await _unitOfWork.UserRepository.GetAsync(id);
        if (responseUser == null)
        {
            return CommonResponse<UserDto>.CreateWarning();
        }

        UserDto mappedUser = _iMapper.Map<UserDto>(responseUser);
        return CommonResponse<UserDto>.CreateSuccess(mappedUser);
    }

    public async Task<CommonResponse<long>> CreateAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default)
    {
        User mappedUser = _iMapper.Map<User>(createUserRequest);
        mappedUser.Password = SecurePasswordHasher.Hash(mappedUser.Password);
        _currentUser.SetInsertedIdentity(mappedUser);

        await _unitOfWork.UserRepository.AddAsync(mappedUser);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(mappedUser.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> UpdateAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default)
    {
        User existUser = await _unitOfWork.UserRepository.GetAsync(updateUserRequest.Id);
        if (existUser == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        User oldValue = existUser;
        User newValue = _iMapper.Map(updateUserRequest, existUser);

        _currentUser.SetUpdatedIdentity(newValue);

        await _unitOfWork.UserRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existUser.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> ChangePasswordAsync(ChangeUserPasswordRequest changeUserPasswordRequest, CancellationToken cancellationToken = default)
    {
        User existUser = await _unitOfWork.UserRepository.GetAsync(changeUserPasswordRequest.Id);
        if (existUser == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        existUser.Password = SecurePasswordHasher.Hash(changeUserPasswordRequest.NewPassword);
        _currentUser.SetUpdatedIdentity(existUser);

        await _unitOfWork.UserRepository.UpdateAsync(existUser);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existUser.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        User existUser = await _unitOfWork.UserRepository.GetAsync(id);
        if (existUser == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        _currentUser.SetDeletedIdentity(existUser);
        await _unitOfWork.UserRepository.DeleteAsync(existUser);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existUser.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    async Task<User> IUserService.FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.UserRepository.Get(x => x.Email == email).SingleOrDefaultAsync();
    }
    async Task IUserService.EditUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<bool> HasPermissionAsync(long userId, string permission, CancellationToken cancellationToken = default)
    {
        //return true;
        return await _unitOfWork.UserRepository
            .Get(x => x.Id == userId)
            .Where(u => u.Id == userId && u.Status == UserStatus.IsActive)
            .SelectMany(u => u.UserRoles
                .Where(ur => ur.Role.Status == EntityStatus.IsActive)
                .SelectMany(ur => ur.Role.RoleActionAuthorizations
                    .Where(raa => raa.ActionAuthorization.Permission == permission && raa.Status == EntityStatus.IsActive)
                )
            )
            .AnyAsync();
    }

}