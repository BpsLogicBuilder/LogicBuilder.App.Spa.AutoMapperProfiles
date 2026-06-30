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
    public class DetailGroupSettingsParametersTest
    {
        static DetailGroupSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var groupTemplate = new DetailGroupTemplateParameters("groupTemplate");
            var fieldSettings = new List<IDetailItemParameters>();

            // Act
            var parameters = new DetailGroupSettingsParameters(
                field: "Address",
                title: "Address Information",
                groupTemplate: groupTemplate,
                fieldSettings: fieldSettings
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailGroupSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.DetailItemType.Group, descriptor.DetailType);
            Assert.Equal("Address", descriptor.Field);
            Assert.Equal("Address Information", descriptor.Title);
            Assert.Equal(groupTemplate.TemplateName, descriptor.GroupTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var groupTemplate = new DetailGroupTemplateParameters("customGroup");
            var fieldSettings = new List<IDetailItemParameters>
            {
                new DetailFieldSettingParameters("Street", "Street", "text", "MyApp.Domain.Entities.Address")
            };

            // Act
            var parameters = new DetailGroupSettingsParameters(
                field: "ShippingAddress",
                title: "Shipping Address",
                groupTemplate: groupTemplate,
                fieldSettings: fieldSettings,
                modelType: "MyApp.Domain.Entities.Order"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailGroupSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.DetailItemType.Group, descriptor.DetailType);
            Assert.Equal("ShippingAddress", descriptor.Field);
            Assert.Equal("Shipping Address", descriptor.Title);
            Assert.Equal(groupTemplate.TemplateName, descriptor.GroupTemplate.TemplateName);
            Assert.Equal(((DetailFieldSettingParameters)fieldSettings[0]).Title, ((DetailFieldSettingDescriptor)descriptor.FieldSettings[0]).Title);
            Assert.Equal("MyApp.Domain.Entities.Order", descriptor.ModelType);
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
