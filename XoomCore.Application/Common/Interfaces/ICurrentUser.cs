using System.Security.Claims;
using XoomCore.Domain.Shared;

namespace XoomCore.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? Name { get; }

    long GetUserId();

    string? GetUserEmail();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();

    void SetInsertedIdentity<TEntity>(TEntity entity) where TEntity : IAuditableEntity;
    void SetUpdatedIdentity<TEntity>(TEntity entity) where TEntity : IAuditableEntity;
    void SetDeletedIdentity<TEntity>(TEntity entity) where TEntity : ISoftDelete;
}