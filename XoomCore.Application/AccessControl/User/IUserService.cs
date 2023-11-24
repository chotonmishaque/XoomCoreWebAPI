using XoomCore.Application.Common.Request;

namespace XoomCore.Application.AccessControl.User;

public interface IUserService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<UserDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default);
    Task<CommonResponse<UserDto>> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> CreateAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> UpdateAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> ChangePasswordAsync(ChangeUserPasswordRequest changeUserPasswordRequest, CancellationToken cancellationToken = default);
    Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default);
    //Task<CommonResponse<SignInResponse>> CreateSessionAsync(SignInRequest signInRequest);
    Task EditUserAsync(XoomCore.Domain.Entities.AccessControl.User user, CancellationToken cancellationToken = default);
    Task<XoomCore.Domain.Entities.AccessControl.User> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(long userId, string permission, CancellationToken cancellationToken = default);

}