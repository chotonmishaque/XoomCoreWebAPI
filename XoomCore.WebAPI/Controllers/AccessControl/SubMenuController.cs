namespace XoomCore.WebAPI.Controllers.AccessControl;

public class SubMenuController : VersionedApiController
{
    private readonly ISubMenuService _subMenuService;
    public SubMenuController(ISubMenuService subMenuService)
    {
        _subMenuService = subMenuService;
    }
    /*****************************
        
        SubMenu Related Action Start

    *****************************/
    [HttpGet("search")]
    [MustActionAuthorized(controller: "SubMenu", action: "SearchAsync")]
    public async Task<CommonDataTableResponse<IEnumerable<SubMenuDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken = default)
    {
        return await _subMenuService.SearchAsync(getDataTableRequest, cancellationToken);
    }

    [HttpGet("{id:long}")]
    [MustActionAuthorized(controller: "SubMenu", action: "GetAsync")]
    [OpenApiOperation("Get Menu", "")]
    public async Task<CommonResponse<SubMenuDto>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _subMenuService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    [MustActionAuthorized(controller: "SubMenu", action: "CreateAsync")]
    [OpenApiOperation("Create Async", "")]
    [ValidateRequest<CreateSubMenuRequest>()]
    public async Task<CommonResponse<long>> CreateAsync([FromBody] CreateSubMenuRequest createSubMenuRequest, CancellationToken cancellationToken = default)
    {
        return await _subMenuService.CreateAsync(createSubMenuRequest, cancellationToken);
    }

    [HttpPut]
    [MustActionAuthorized(controller: "SubMenu", action: "UpdateAsync")]
    [OpenApiOperation("Update Async", "")]
    [ValidateRequest<UpdateSubMenuRequest>()]
    public async Task<CommonResponse<long>> UpdateAsync([FromBody] UpdateSubMenuRequest updateSubMenuRequest, CancellationToken cancellationToken = default)
    {
        return await _subMenuService.UpdateAsync(updateSubMenuRequest, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    [MustActionAuthorized(controller: "SubMenu", action: "DeleteAsync")]
    [OpenApiOperation("Delete Async", "")]
    public async Task<CommonResponse<long>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _subMenuService.DeleteAsync(id, cancellationToken);
    }

    /*****************************

       SubMenu Related Action End

    *****************************/
}
