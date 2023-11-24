using XoomCore.Application.Report;
using XoomCore.Domain.Entities.Report;

namespace XoomCore.Services.Mapper;

public class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {

        CreateMap<EntityLog, EntityLogDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.CreatedByUser.Username));

        // Add more mappings as needed
    }
}
