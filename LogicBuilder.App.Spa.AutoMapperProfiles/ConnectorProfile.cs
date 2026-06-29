using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration;
using LogicBuilder.App.Spa.Forms.Parameters;
using LogicBuilder.Forms.Parameters;

namespace LogicBuilder.App.Spa.AutoMapperProfiles
{
    public class ConnectorProfile : Profile
    {
        public ConnectorProfile()
        {
            CreateMap<ConnectorParameters, CommandButtonDescriptor>()
                .ForCtorParam("buttonIcon", opts => opts.MapFrom(src => src.ConnectorData == null ? "" : ((CommandButtonParameters)src.ConnectorData).ButtonIcon))
                .ForCtorParam("cancel", opts => opts.MapFrom(src => (src.ConnectorData != null) && ((CommandButtonParameters)src.ConnectorData).Cancel))
                .ForCtorParam("classString", opts => opts.MapFrom(src => src.ConnectorData == null ? "" : ((CommandButtonParameters)src.ConnectorData).ClassString))
                .ForCtorParam("gridCommandButton", opts => opts.MapFrom(src => src.ConnectorData == null ? null : ((CommandButtonParameters)src.ConnectorData).GridCommandButton))
                .ForCtorParam("gridId", opts => opts.MapFrom(src => src.ConnectorData == null ? null : ((CommandButtonParameters)src.ConnectorData).GridId));
            CreateMap<CommandButtonParameters, CommandButtonDescriptor>()
                .ForCtorParam("id", opt => opt.MapFrom(src => 0))
                .ForCtorParam("shortString", opt => opt.MapFrom(src => ""))
                .ForCtorParam("longString", opt => opt.MapFrom(src => ""));
        }
    }
}
