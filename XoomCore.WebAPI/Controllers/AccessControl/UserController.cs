using XoomCore.Application.AccessControl.User;
using XoomCore.WebAPI.Controllers;

namespace XoomCore.Web.AccessControl.Controllers;

public class UserController : VersionedApiController
{

    private readonly IUserService _UserService;
    public UserController(
        IUserService UserService
        )
    {
        _UserService = UserService;
    }
    /*****************************
        
        User Related Action Start

    *****************************/

    [HttpGet("search")]
    [MustActionAuthorized(controller: "User", action: "SearchAsync")]
    [OpenApiOperation("Search ActionAuthorization", "")]
    [ValidateRequest<GetDataTableRequest>()]
    public async Task<CommonDataTableResponse<IEnumerable<UserDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        return await _UserService.SearchAsync(getDataTableRequest, cancellationToken);
    }

    [HttpGet("{id:long}")]
    [MustActionAuthorized(controller: "User", action: "GetAsync")]
    [OpenApiOperation("Get ActionAuthorization", "")]
    public async Task<CommonResponse<UserDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _UserService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    [MustActionAuthorized(controller: "User", action: "CreateAsync")]
    [OpenApiOperation("Create Async", "")]
    [ValidateRequest<CreateUserRequest>()]
    public async Task<CommonResponse<long>> CreateAsync([FromBody] CreateUserRequest postUserRequest, CancellationToken cancellationToken = default)
    {
        return await _UserService.CreateAsync(postUserRequest, cancellationToken);
    }


    [HttpPut]
    [MustActionAuthorized(controller: "User", action: "UpdateAsync")]
    [OpenApiOperation("Update Async", "")]
    [ValidateRequest<UpdateUserRequest>()]
    public async Task<CommonResponse<long>> UpdateAsync([FromBody] UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default)
    {
        return await _UserService.UpdateAsync(updateUserRequest, cancellationToken);
    }

    [HttpPut("ChangePassword")]
    [MustActionAuthorized(controller: "User", action: "ChangePasswordAsync")]
    [OpenApiOperation("ChangePassword Async", "")]
    [ValidateRequest<ChangeUserPasswordRequest>()]
    public async Task<CommonResponse<long>> ChangePasswordAsync([FromBody] ChangeUserPasswordRequest changeUserPasswordRequest, CancellationToken cancellationToken = default)
    {
        return await _UserService.ChangePasswordAsync(changeUserPasswordRequest, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    [MustActionAuthorized(controller: "User", action: "DeleteAsync")]
    [OpenApiOperation("Delete Async", "")]
    public async Task<CommonResponse<long>> DeleteUser(long id, CancellationToken cancellationToken = default)
    {
        return await _UserService.DeleteAsync(id, cancellationToken);
    }

    /*****************************

       User Related Action End

    *****************************/
}

