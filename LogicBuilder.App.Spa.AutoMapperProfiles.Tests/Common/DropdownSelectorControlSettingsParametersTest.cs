using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Forms.Parameters.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class DropdownSelectorControlSettingsParametersTest
    {
        static DropdownSelectorControlSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Status",
                "MyApp.Data.Entities.Status",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.Generic.IEnumerable`1"
            );
            var dropDownTemplate = new DropDownTemplateParameters(
                "dropDownTemplate",
                "Select...",
                "Name",
                "Id",
                textAndValueSelector,
                requestDetails
            );

            // Act
            var parameters = new DropdownSelectorControlSettingsParameters(
                field: "StatusId",
                domElementId: "statusId_id",
                title: "Status",
                placeHolder: "Select status",
                type: "numeric",
                dropDownTemplate: dropDownTemplate,
                modelType: "MyApp.Domain.Entities.Order"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DropdownSelectorControlSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.DropdownSelectorControl, descriptor.AbstractControlType);
            Assert.Equal("StatusId", descriptor.Field);
            Assert.Equal("statusId_id", descriptor.DomElementId);
            Assert.Equal("Status", descriptor.Title);
            Assert.Equal("Select status", descriptor.Placeholder);
            Assert.Equal("numeric", descriptor.Type);
            Assert.Equal(dropDownTemplate.TemplateName, descriptor.DropDownTemplate.TemplateName);
            Assert.Equal("MyApp.Domain.Entities.Order", descriptor.ModelType);
            Assert.Null(descriptor.ValidationSetting);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Category",
                "MyApp.Data.Entities.Category",
                "System.Collections.Generic.List`1",
                "System.Collections.Generic.List`1"
            );
            var dropDownTemplate = new DropDownTemplateParameters(
                "categoryDropDown",
                "Choose...",
                "CategoryName",
                "CategoryId",
                textAndValueSelector,
                requestDetails
            );
            var validationSetting = new FormValidationSettingParameters(defaultValue: 1);

            // Act
            var parameters = new DropdownSelectorControlSettingsParameters(
                field: "CategoryId",
                domElementId: "categoryId_id",
                title: "Category",
                placeHolder: "Select category",
                type: "numeric",
                dropDownTemplate: dropDownTemplate,
                modelType: "MyApp.Domain.Entities.Product",
                validationSetting: validationSetting
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DropdownSelectorControlSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.DropdownSelectorControl, descriptor.AbstractControlType);
            Assert.Equal("CategoryId", descriptor.Field);
            Assert.Equal("categoryId_id", descriptor.DomElementId);
            Assert.Equal("Category", descriptor.Title);
            Assert.Equal("Select category", descriptor.Placeholder);
            Assert.Equal("numeric", descriptor.Type);
            Assert.Equal(dropDownTemplate.TemplateName, descriptor.DropDownTemplate.TemplateName);
            Assert.Equal("MyApp.Domain.Entities.Product", descriptor.ModelType);
            Assert.Equal(validationSetting.DefaultValue, descriptor.ValidationSetting!.DefaultValue);
        }

        #region Helpers
        [MemberNotNull(nameof(MapperConfiguration))]
        [MemberNotNull(nameof(serviceProvider))]
        private static void Initialize()
        {
            MapperConfiguration ??= new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BaseClassMappings>();
                cfg.AddProfile<ConnectorProfile>();
                cfg.AddProfile<ParameterToDescriptorProfile>();
                cfg.AddProfile<ExpansionParameterToDescriptorMappingProfile>();
                cfg.AddProfile<ExpressionParameterToDescriptorMappingProfile>();
            }, NullLoggerFactory.Instance);
            MapperConfiguration.AssertConfigurationIsValid();

            serviceProvider ??= new ServiceCollection()
                .AddSingleton<AutoMapper.IConfigurationProvider>
                (
                    MapperConfiguration
                )
                .AddTransient<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService))
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
