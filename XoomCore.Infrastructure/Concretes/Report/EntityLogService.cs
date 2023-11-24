using XoomCore.Application.Common.Request;
using XoomCore.Application.Report;
using XoomCore.Domain.Entities.Report;

namespace XoomCore.Services.Concretes.Report;

public class EntityLogService : IEntityLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;
    public EntityLogService(IUnitOfWork unitOfWork, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<EntityLogDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getEntityLogListQuery = _unitOfWork.EntityLogRepository
                    .GetAll(cancellationToken)
                    .Where(x => x.EntityName.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        if (await getEntityLogListQuery.LongCountAsync() is long totalRowCount && totalRowCount < 1)
        {
            return CommonDataTableResponse<IEnumerable<EntityLogDto>>.CreateWarning();
        }
        List<EntityLog> responseEntityLogList = await getEntityLogListQuery
                                    .OrderByDescending(x => x.Id)
                                    .Include(x => x.CreatedByUser)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync(cancellationToken);

        IEnumerable<EntityLogDto> mappedEntityLogList = _iMapper.Map<List<EntityLogDto>>(responseEntityLogList);

        return CommonDataTableResponse<IEnumerable<EntityLogDto>>.CreateSuccess(totalRowCount, responseEntityLogList.LongCount(), mappedEntityLogList);
    }
}
