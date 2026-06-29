using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;

namespace LogicBuilder.App.Spa.AutoMapperProfiles
{
    public class BaseClassMappings : Profile
    {
        public BaseClassMappings()
        {
            CreateMap<IFormItemSettingParameters, FormItemSettingDescriptor>()
                .Include<DropdownSelectorControlSettingsParameters, DropdownSelectorControlSettingsDescriptor>()
                .Include<FormGroupArraySettingsParameters, FormGroupArraySettingsDescriptor>()
                .Include<FormGroupSettingsParameters, FormGroupSettingsDescriptor>()
                .Include<FormGroupBoxSettingsParameters, FormGroupBoxSettingsDescriptor>()
                .Include<InputFieldControlSettingsParameters, InputFieldControlSettingsDescriptor>()
                .Include<MultiSelectFormControlSettingsParameters, MultiSelectFormControlSettingsDescriptor>();

            CreateMap<IDetailItemParameters, DetailItemDescriptor>()
                .Include<DetailFieldSettingParameters, DetailFieldSettingDescriptor>()
                .Include<DetailGroupSettingsParameters, DetailGroupSettingsDescriptor>()
                .Include<DetailListSettingsParameters, DetailListSettingsDescriptor>();
        }
    }
}
