using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.EntityFrameworkCore.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class InputFieldControlSettingsParametersTest
    {
        static InputFieldControlSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textTemplate = new TextFieldTemplateParameters("textTemplate");

            // Act
            var parameters = new InputFieldControlSettingsParameters(
                field: "FirstName",
                domElementId: "firstName_id",
                title: "First Name",
                placeHolder: "Enter first name",
                type: "text",
                textTemplate: textTemplate,
                modelType: "MyApp.Domain.Entities.Student"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<InputFieldControlSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.InputFieldControl, descriptor.AbstractControlType);
            Assert.Equal("FirstName", descriptor.Field);
            Assert.Equal("firstName_id", descriptor.DomElementId);
            Assert.Equal("First Name", descriptor.Title);
            Assert.Equal("Enter first name", descriptor.Placeholder);
            Assert.Equal("text", descriptor.Type);
            Assert.Equal(textTemplate.TemplateName, descriptor.TextTemplate.TemplateName);
            Assert.Equal("MyApp.Domain.Entities.Student", descriptor.ModelType);
            Assert.Null(descriptor.ValidationSetting);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textTemplate = new TextFieldTemplateParameters("customTemplate");
            var validationSetting = new FormValidationSettingParameters(defaultValue: "Default");

            // Act
            var parameters = new InputFieldControlSettingsParameters(
                field: "Email",
                domElementId: "email_id",
                title: "Email Address",
                placeHolder: "Enter email",
                type: "email",
                textTemplate: textTemplate,
                modelType: "MyApp.Domain.Entities.Contact",
                validationSetting: validationSetting
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<InputFieldControlSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.InputFieldControl, descriptor.AbstractControlType);
            Assert.Equal("Email", descriptor.Field);
            Assert.Equal("email_id", descriptor.DomElementId);
            Assert.Equal("Email Address", descriptor.Title);
            Assert.Equal("Enter email", descriptor.Placeholder);
            Assert.Equal("email", descriptor.Type);
            Assert.Equal(textTemplate.TemplateName, descriptor.TextTemplate.TemplateName);
            Assert.Equal("MyApp.Domain.Entities.Contact", descriptor.ModelType);
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
