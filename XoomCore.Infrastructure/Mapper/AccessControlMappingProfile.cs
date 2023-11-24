using XoomCore.Application.AccessControl.ActionAuthorization;
using XoomCore.Application.AccessControl.Menu;
using XoomCore.Application.AccessControl.Role;
using XoomCore.Application.AccessControl.RoleActionAuthorization;
using XoomCore.Application.AccessControl.SubMenu;
using XoomCore.Application.AccessControl.User;
using XoomCore.Application.AccessControl.UserRole;

namespace XoomCore.Services.Mapper;

public class AccessControlMappingProfile : Profile
{
    public AccessControlMappingProfile()
    {
        CreateMap<CreateMenuRequest, Menu>();
        CreateMap<UpdateMenuRequest, Menu>();
        CreateMap<Menu, MenuDto>();
        CreateMap<Menu, SelectOptionResponse>();

        CreateMap<CreateSubMenuRequest, SubMenu>();
        CreateMap<UpdateSubMenuRequest, SubMenu>();
        CreateMap<SubMenu, SubMenuDto>()
            .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.Menu.Name));
        CreateMap<SubMenu, SelectOptionResponse>();

        CreateMap<CreateActionAuthorizationRequest, ActionAuthorization>();
        CreateMap<UpdateActionAuthorizationRequest, ActionAuthorization>();
        CreateMap<ActionAuthorization, ActionAuthorizationDto>()
            .ForMember(dest => dest.MenuId, opt => opt.MapFrom(src => src.SubMenu.MenuId))
            .ForMember(dest => dest.MenuName, opt => opt.MapFrom(src => src.SubMenu.Menu.Name))
            .ForMember(dest => dest.SubMenuName, opt => opt.MapFrom(src => src.SubMenu.Name));
        CreateMap<ActionAuthorization, SelectOptionResponse>();

        CreateMap<CreateRoleActionAuthorizationRequest, RoleActionAuthorization>();
        CreateMap<UpdateRoleActionAuthorizationRequest, RoleActionAuthorization>();
        CreateMap<RoleActionAuthorization, RoleActionAuthorizationDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.ActionAuthorizationName, opt => opt.MapFrom(src => src.ActionAuthorization.Name))
            .ForMember(dest => dest.Controller, opt => opt.MapFrom(src => src.ActionAuthorization.Controller))
            .ForMember(dest => dest.ActionMethod, opt => opt.MapFrom(src => src.ActionAuthorization.ActionMethod));

        CreateMap<CreateRoleRequest, Role>();
        CreateMap<UpdateRoleRequest, Role>();
        CreateMap<Role, RoleDto>();
        CreateMap<Role, SelectOptionResponse>();

        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();
        CreateMap<User, UserDto>();
        CreateMap<User, SelectOptionResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName + " (" + src.Username + ")"));


        CreateMap<CreateUserRoleRequest, UserRole>();
        CreateMap<UpdateUserRoleRequest, UserRole>();
        CreateMap<UserRole, UserRoleDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

    }
}