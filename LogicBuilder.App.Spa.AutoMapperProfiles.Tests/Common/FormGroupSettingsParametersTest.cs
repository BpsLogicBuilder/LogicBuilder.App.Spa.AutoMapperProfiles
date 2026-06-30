using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class FormGroupSettingsParametersTest
    {
        static FormGroupSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var formGroupTemplate = new FormGroupTemplateParameters("groupTemplate");
            var fieldSettings = new List<IFormItemSettingParameters>();
            var validationMessages = new List<ValidationMessageParameters>();

            // Act
            var parameters = new FormGroupSettingsParameters(
                field: "Address",
                formGroupTemplate: formGroupTemplate,
                fieldSettings: fieldSettings,
                validationMessages: validationMessages,
                title: "Address Information",
                showTitle: true,
                modelType: "MyApp.Domain.Entities.Customer"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormGroupSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.FormGroup, descriptor.AbstractControlType);
            Assert.Equal("Address", descriptor.Field);
            Assert.Equal(formGroupTemplate.TemplateName, descriptor.FormGroupTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.NotNull(descriptor.ValidationMessages);
            Assert.NotNull(descriptor.ConditionalDirectives);
            Assert.Equal("Address Information", descriptor.Title);
            Assert.True(descriptor.ShowTitle);
            Assert.Equal("MyApp.Domain.Entities.Customer", descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var formGroupTemplate = new FormGroupTemplateParameters("contactTemplate");
            var fieldSettings = new List<IFormItemSettingParameters>();
            var validationMessages = new List<ValidationMessageParameters>
            {
                new("Email",
                [
                    new ValidationMethodParameters("email", "Invalid email format") 
                ])
            };
            var conditionalDirectives = new List<VariableDirectivesParameters>
            {
                new("Phone", [])
            };

            // Act
            var parameters = new FormGroupSettingsParameters(
                field: "ContactInfo",
                formGroupTemplate: formGroupTemplate,
                fieldSettings: fieldSettings,
                validationMessages: validationMessages,
                title: "Contact Details",
                showTitle: false,
                modelType: "MyApp.Domain.Entities.Person",
                conditionalDirectives: conditionalDirectives
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormGroupSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.FormGroup, descriptor.AbstractControlType);
            Assert.Equal("ContactInfo", descriptor.Field);
            Assert.Equal(formGroupTemplate.TemplateName, descriptor.FormGroupTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.NotNull(descriptor.ValidationMessages);
            Assert.NotNull(descriptor.ConditionalDirectives);
            Assert.Equal("Contact Details", descriptor.Title);
            Assert.False(descriptor.ShowTitle);
            Assert.Equal("MyApp.Domain.Entities.Person", descriptor.ModelType);
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
            }, NullLoggerFactory.Instance);

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
