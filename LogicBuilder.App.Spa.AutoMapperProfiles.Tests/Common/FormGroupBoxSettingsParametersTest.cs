using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.EntityFrameworkCore.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class FormGroupBoxSettingsParametersTest
    {
        static FormGroupBoxSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var formGroupTemplate = new FormGroupTemplateParameters("boxTemplate");
            var fieldSettings = new List<IFormItemSettingParameters>();

            // Act
            var parameters = new FormGroupBoxSettingsParameters(
                formGroupTemplate: formGroupTemplate,
                fieldSettings: fieldSettings,
                title: "Contact Information"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormGroupBoxSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.GroupBox, descriptor.AbstractControlType);
            Assert.Equal(formGroupTemplate.TemplateName, descriptor.FormGroupTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.Equal("Contact Information", descriptor.Title);
            Assert.True(descriptor.ShowTitle);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var formGroupTemplate = new FormGroupTemplateParameters("customBox");
            var fieldSettings = new List<IFormItemSettingParameters>();

            // Act
            var parameters = new FormGroupBoxSettingsParameters(
                formGroupTemplate: formGroupTemplate,
                fieldSettings: fieldSettings,
                title: "Address Details",
                showTitle: false
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormGroupBoxSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.GroupBox, descriptor.AbstractControlType);
            Assert.Equal(formGroupTemplate.TemplateName, descriptor.FormGroupTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.Equal("Address Details", descriptor.Title);
            Assert.False(descriptor.ShowTitle);
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
