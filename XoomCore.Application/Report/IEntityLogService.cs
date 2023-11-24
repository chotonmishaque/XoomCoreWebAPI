using XoomCore.Application.Common.Request;

namespace XoomCore.Application.Report;

public interface IEntityLogService : ITransientService
{
    Task<CommonDataTableResponse<IEnumerable<EntityLogDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default);
}
