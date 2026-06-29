using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;

namespace LogicBuilder.App.Spa.AutoMapperProfiles
{
    public class ParameterToDescriptorProfile : Profile
    {
        public ParameterToDescriptorProfile()
        {
            CreateMap<ListFormSettingsParameters, ListFormSettingsDescriptor>();
            CreateMap<AggregateDefinitionParameters, AggregateDefinitionDescriptor>();
            CreateMap<AggregateTemplateFieldsParameters, AggregateTemplateFieldsDescriptor>();
            CreateMap<AggregateTemplateParameters, AggregateTemplateDescriptor>();
            CreateMap<CellListTemplateParameters, CellListTemplateDescriptor>();
            CreateMap<CellTemplateParameters, CellTemplateDescriptor>();
            CreateMap<ColumnSettingsParameters, ColumnSettingsDescriptor>();
            CreateMap<CommandColumnParameters, CommandColumnDescriptor>();
            CreateMap<ConditionGroupParameters, ConditionGroupDescriptor>();
            CreateMap<ConditionParameters, ConditionDescriptor>();
            CreateMap<ContentTemplateParameters, ContentTemplateDescriptor>();
            CreateMap<DataRequestStateParameters, DataRequestStateDescriptor>();
            CreateMap<DetailDropDownTemplateParameters, DetailDropDownTemplateDescriptor>();
            CreateMap<DetailFieldSettingParameters, DetailFieldSettingDescriptor>();
            CreateMap<DetailFieldTemplateParameters, DetailFieldTemplateDescriptor>();
            CreateMap<DetailFormSettingsParameters, DetailFormSettingsDescriptor>();
            CreateMap<DetailGroupSettingsParameters, DetailGroupSettingsDescriptor>();
            CreateMap<DetailGroupTemplateParameters, DetailGroupTemplateDescriptor>();
            CreateMap<DetailListSettingsParameters, DetailListSettingsDescriptor>();
            CreateMap<DetailListTemplateParameters, DetailListTemplateDescriptor>();
            CreateMap<DirectiveArgumentParameters, DirectiveArgumentDescriptor>();
            CreateMap<DirectiveDescriptionParameters, DirectiveDescriptionDescriptor>();
            CreateMap<DirectiveParameters, DirectiveDescriptor>();
            CreateMap<DomainRequestParameters, DomainRequestDescriptor>();
            CreateMap<DropdownSelectorControlSettingsParameters, DropdownSelectorControlSettingsDescriptor>();
            CreateMap<DropDownTemplateParameters, DropDownTemplateDescriptor>();
            CreateMap<DummyConstructor, DummyConstructor>();
            CreateMap<EditFormSettingsParameters, EditFormSettingsDescriptor>();
            CreateMap<FilterDefinitionParameters, FilterDefinitionDescriptor>();
            CreateMap<FilterGroupParameters, FilterGroupDescriptor>();
            CreateMap<FilterTemplateParameters, FilterTemplateDescriptor>();
            CreateMap<FormGroupArraySettingsParameters, FormGroupArraySettingsDescriptor>();
            CreateMap<FormGroupBoxSettingsParameters, FormGroupBoxSettingsDescriptor>();
            CreateMap<FormGroupSettingsParameters, FormGroupSettingsDescriptor>();
            CreateMap<FormGroupTemplateParameters, FormGroupTemplateDescriptor>();
            CreateMap<FormRequestDetailsParameters, FormRequestDetailsDescriptor>();
            CreateMap<FormValidationSettingParameters, FormValidationSettingDescriptor>();
            CreateMap<GridRequestDetailsParameters, GridRequestDetailsDescriptor>();
            CreateMap<GridSettingsParameters, GridSettingsDescriptor>();
            CreateMap<GroupParameters, GroupDescriptor>();
            CreateMap<HtmlPageSettingsParameters, HtmlPageSettingsDescriptor>();
            CreateMap<InputFieldControlSettingsParameters, InputFieldControlSettingsDescriptor>();
            CreateMap<MessageTemplateParameters, MessageTemplateDescriptor>();
            CreateMap<MultiSelectFormControlSettingsParameters, MultiSelectFormControlSettingsDescriptor>();
            CreateMap<MultiSelectTemplateParameters, MultiSelectTemplateDescriptor>();
            CreateMap<RequestDetailsParameters, RequestDetailsDescriptor>();
            CreateMap<SelectParameters, SelectDescriptor>();
            CreateMap<SortParameters, SortDescriptor>();
            CreateMap<TextFieldTemplateParameters, TextFieldTemplateDescriptor>();
            CreateMap<ValidationMessageParameters, ValidationMessageDescriptor>();
            CreateMap<ValidationMethodParameters, ValidationMethodDescriptor>();
            CreateMap<ValidatorArgumentParameters, ValidatorArgumentDescriptor>();
            CreateMap<ValidatorDescriptionParameters, ValidatorDescriptionDescriptor>();
            CreateMap<VariableDirectivesParameters, VariableDirectivesDescriptor>();
        }
    }
}
