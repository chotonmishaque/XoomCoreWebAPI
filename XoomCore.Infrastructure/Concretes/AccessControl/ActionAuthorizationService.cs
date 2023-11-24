using XoomCore.Application.AccessControl.ActionAuthorization;
using XoomCore.Application.Common.Request;

namespace XoomCore.Services.Concretes.AccessControl;

public class ActionAuthorizationService : IActionAuthorizationService
{
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;
    public ActionAuthorizationService(ICurrentUser currentUser, IUnitOfWork unitOfWork, IMapper iMapper)
    {
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<ActionAuthorizationDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getActionAuthorizationListQuery = _unitOfWork.ActionAuthorizationRepository
                    .GetAll(cancellationToken)
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        if (await getActionAuthorizationListQuery.LongCountAsync() is long totalRowCount && totalRowCount < 1)
        {
            return CommonDataTableResponse<IEnumerable<ActionAuthorizationDto>>.CreateWarning();
        }
        List<ActionAuthorization> responseActionAuthorizationList = await getActionAuthorizationListQuery
                                    .Include(x => x.SubMenu)
                                        .ThenInclude(x => x.Menu)
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync(cancellationToken);

        IEnumerable<ActionAuthorizationDto> mappedActionAuthorizationList = _iMapper.Map<List<ActionAuthorizationDto>>(responseActionAuthorizationList);

        return CommonDataTableResponse<IEnumerable<ActionAuthorizationDto>>.CreateSuccess(totalRowCount, mappedActionAuthorizationList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default)
    {
        List<ActionAuthorization> responseActionAuthorizationList = await _unitOfWork.ActionAuthorizationRepository.GetAll(cancellationToken).ToListAsync();
        if (!responseActionAuthorizationList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarning();
        }

        IEnumerable<SelectOptionResponse> mappedActionAuthorizationList = _iMapper.Map<List<SelectOptionResponse>>(responseActionAuthorizationList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateSuccess(mappedActionAuthorizationList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<ActionAuthorizationDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        ActionAuthorization responseActionAuthorization = await _unitOfWork.ActionAuthorizationRepository.GetAsync(id, cancellationToken);

        if (responseActionAuthorization == null)
        {
            return CommonResponse<ActionAuthorizationDto>.CreateWarning();
        }

        ActionAuthorizationDto mappedActionAuthorization = _iMapper.Map<ActionAuthorizationDto>(responseActionAuthorization);
        return CommonResponse<ActionAuthorizationDto>.CreateSuccess(mappedActionAuthorization);
    }

    public async Task<CommonResponse<long>> CreateAsync(CreateActionAuthorizationRequest createActionAuthorizationRequest, CancellationToken cancellationToken = default)
    {
        ActionAuthorization mappedActionAuthorization = _iMapper.Map<ActionAuthorization>(createActionAuthorizationRequest);
        _currentUser.SetInsertedIdentity(mappedActionAuthorization);

        await _unitOfWork.ActionAuthorizationRepository.AddAsync(mappedActionAuthorization, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(mappedActionAuthorization.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> UpdateAsync(UpdateActionAuthorizationRequest updateActionAuthorizationRequest, CancellationToken cancellationToken = default)
    {
        ActionAuthorization existActionAuthorization = await _unitOfWork.ActionAuthorizationRepository.GetAsync(updateActionAuthorizationRequest.Id, cancellationToken);
        if (existActionAuthorization == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        ActionAuthorization oldValue = existActionAuthorization;
        ActionAuthorization newValue = _iMapper.Map(updateActionAuthorizationRequest, existActionAuthorization);

        _currentUser.SetUpdatedIdentity(newValue);

        await _unitOfWork.ActionAuthorizationRepository.UpdateAsync(oldValue, newValue, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existActionAuthorization.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        ActionAuthorization existActionAuthorization = await _unitOfWork.ActionAuthorizationRepository.GetAsync(id, cancellationToken);
        if (existActionAuthorization == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }

        await _unitOfWork.ActionAuthorizationRepository.DeleteAsync(existActionAuthorization);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existActionAuthorization.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }
}
