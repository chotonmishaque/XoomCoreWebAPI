using System.Security.Claims;
using XoomCore.Domain.Shared;
using XoomCore.Infrastructure.Auth.Jwt;

namespace XoomCore.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private long _userId = 0;
    public long GetUserId() =>
    IsAuthenticated()
        ? long.TryParse(_user?.GetUserId(), out var result) ? result : 0
        : _userId;

    public string? GetUserEmail() =>
        IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != 0)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            if (long.TryParse(userId, out var parsedUserId))
            {
                _userId = parsedUserId;
            }
            else
            {
                throw new ArgumentException("Invalid userId format");
            }
        }
    }
    public void SetInsertedIdentity<TEntity>(TEntity entity) where TEntity : IAuditableEntity
    {
        entity.CreatedBy = GetUserId();
        entity.CreatedAt = DateTime.UtcNow;

    }
    public void SetUpdatedIdentity<TEntity>(TEntity entity) where TEntity : IAuditableEntity
    {
        entity.UpdatedBy = GetUserId();
        entity.UpdatedAt = DateTime.UtcNow;

    }
    public void SetDeletedIdentity<TEntity>(TEntity entity) where TEntity : ISoftDelete
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        // Check if the entity is an instance of AuditableEntity
        if (entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.DeletedBy = GetUserId();
            softDeleteEntity.DeletedAt = DateTime.UtcNow;
            // Set other audit-related properties if needed
        }
    }
}