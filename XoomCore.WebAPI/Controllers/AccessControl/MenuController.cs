namespace XoomCore.WebAPI.Controllers.AccessControl.Controllers;

public class MenuController : VersionedApiController
{
    private readonly IMenuService _menuService;
    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    /*****************************
        
        Menu Related Action Start

    *****************************/

    [HttpGet("search")]
    [MustActionAuthorized(controller: "Menu", action: "SearchAsync")]
    [OpenApiOperation("Search Menu", "")]
    [ValidateRequest<GetDataTableRequest>()]
    public async Task<CommonDataTableResponse<IEnumerable<MenuDto>>> SearchAsync([FromQuery] GetDataTableRequest getDataTableRequest, CancellationToken cancellationToken)
    {
        return await _menuService.SearchAsync(getDataTableRequest, cancellationToken);
    }

    [HttpGet("{id:long}")]
    [MustActionAuthorized(controller: "Menu", action: "GetAsync")]
    [OpenApiOperation("Get Menu", "")]
    public async Task<CommonResponse<MenuDto>> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await _menuService.GetAsync(id, cancellationToken);
    }

    [HttpPost]
    [MustActionAuthorized(controller: "Menu", action: "CreateAsync")]
    [OpenApiOperation("Create Async", "")]
    [ValidateRequest<CreateMenuRequest>()]
    public async Task<CommonResponse<long>> CreateAsync([FromBody] CreateMenuRequest createMenuRequest, CancellationToken cancellationToken)
    {
        return await _menuService.CreateAsync(createMenuRequest, cancellationToken);
    }

    [HttpPut]
    [MustActionAuthorized(controller: "Menu", action: "UpdateAsync")]
    [OpenApiOperation("Update Async", "")]
    [ValidateRequest<UpdateMenuRequest>()]
    public async Task<CommonResponse<long>> UpdateAsync([FromBody] UpdateMenuRequest updateMenuRequest, CancellationToken cancellationToken)
    {
        return await _menuService.UpdateAsync(updateMenuRequest, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    [MustActionAuthorized(controller: "Menu", action: "DeleteAsync")]
    [OpenApiOperation("Delete Async", "")]
    public async Task<CommonResponse<long>> DeleteMenu(long id, CancellationToken cancellationToken)
    {
        return await _menuService.DeleteAsync(id, cancellationToken);
    }

    /*****************************

       Menu Related Action End

    *****************************/
}
