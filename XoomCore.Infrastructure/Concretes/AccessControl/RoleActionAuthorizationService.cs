using XoomCore.Application.AccessControl.RoleActionAuthorization;
using XoomCore.Application.Common.Request;

namespace XoomCore.Services.Concretes.AccessControl;

public class RoleActionAuthorizationService : IRoleActionAuthorizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _iMapper;
    public RoleActionAuthorizationService(IUnitOfWork unitOfWork, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<RoleActionAuthorizationDto>>> SearchAsync(GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        var getRoleActionAuthorizationListQuery = _unitOfWork.RoleActionAuthorizationRepository
                    .GetAll(cancellationToken)
                    .Where(x => x.Role.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        if (await getRoleActionAuthorizationListQuery.LongCountAsync() is long totalRowCount && totalRowCount < 1)
        {
            return CommonDataTableResponse<IEnumerable<RoleActionAuthorizationDto>>.CreateWarning();
        }

        List<RoleActionAuthorization> responseRoleActionAuthorizationList = await getRoleActionAuthorizationListQuery
                                    .Include(x => x.Role)
                                    .Include(x => x.ActionAuthorization)
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync(cancellationToken);

        IEnumerable<RoleActionAuthorizationDto> mappedRoleActionAuthorizationList = _iMapper.Map<List<RoleActionAuthorizationDto>>(responseRoleActionAuthorizationList);

        return CommonDataTableResponse<IEnumerable<RoleActionAuthorizationDto>>.CreateSuccess(totalRowCount, mappedRoleActionAuthorizationList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetForSelectAsync(CancellationToken cancellationToken = default)
    {
        List<RoleActionAuthorization> responseRoleActionAuthorizationList = await _unitOfWork.RoleActionAuthorizationRepository.GetAll().ToListAsync();
        if (!responseRoleActionAuthorizationList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarning();
        }

        IEnumerable<SelectOptionResponse> mappedRoleActionAuthorizationList = _iMapper.Map<List<SelectOptionResponse>>(responseRoleActionAuthorizationList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateSuccess(mappedRoleActionAuthorizationList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<RoleActionAuthorizationDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        RoleActionAuthorization responseRoleActionAuthorization = await _unitOfWork.RoleActionAuthorizationRepository
                                                                .Get(x => x.Id == id, cancellationToken)
                                                                .Include(x => x.ActionAuthorization)
                                                                .SingleOrDefaultAsync(cancellationToken);
        if (responseRoleActionAuthorization == null)
        {
            return CommonResponse<RoleActionAuthorizationDto>.CreateWarning();
        }

        RoleActionAuthorizationDto mappedRoleActionAuthorization = _iMapper.Map<RoleActionAuthorizationDto>(responseRoleActionAuthorization);
        return CommonResponse<RoleActionAuthorizationDto>.CreateSuccess(mappedRoleActionAuthorization);
    }

    public async Task<CommonResponse<long>> CreateAsync(CreateRoleActionAuthorizationRequest createRoleActionAuthorizationRequest, CancellationToken cancellationToken = default)
    {
        RoleActionAuthorization mappedRoleActionAuthorization = _iMapper.Map<RoleActionAuthorization>(createRoleActionAuthorizationRequest);
        //_sessionManager.SetInsertedIdentity(mappedRoleActionAuthorization);

        await _unitOfWork.RoleActionAuthorizationRepository.AddAsync(mappedRoleActionAuthorization, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(mappedRoleActionAuthorization.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> UpdateAsync(UpdateRoleActionAuthorizationRequest updateRoleActionAuthorizationRequest, CancellationToken cancellationToken = default)
    {
        RoleActionAuthorization existRoleActionAuthorization = await _unitOfWork.RoleActionAuthorizationRepository.GetAsync(updateRoleActionAuthorizationRequest.Id, cancellationToken);
        if (existRoleActionAuthorization == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }
        RoleActionAuthorization oldValue = existRoleActionAuthorization;
        RoleActionAuthorization newValue = _iMapper.Map(updateRoleActionAuthorizationRequest, existRoleActionAuthorization);
        //updateRoleActionAuthorizationRequest.MapTo(existRoleActionAuthorization);

        //_sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.RoleActionAuthorizationRepository.UpdateAsync(oldValue, newValue, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existRoleActionAuthorization.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        RoleActionAuthorization existRoleActionAuthorization = await _unitOfWork.RoleActionAuthorizationRepository.GetAsync(id);
        if (existRoleActionAuthorization == null)
        {
            return CommonResponse<long>.CreateWarning(message: "Invalid request detected!");
        }

        await _unitOfWork.RoleActionAuthorizationRepository.DeleteAsync(existRoleActionAuthorization, cancellationToken);
        int dbCode = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateSuccess(existRoleActionAuthorization.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateError(dbCode);
    }
}
