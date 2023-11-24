using XoomCore.Persistence.Common.Interfaces;

namespace XoomCore.Persistence.Repositories.Contracts.AccessControl;

public interface IActionAuthorizationRepository : IRepository<ActionAuthorization>, IScopedService
{

}