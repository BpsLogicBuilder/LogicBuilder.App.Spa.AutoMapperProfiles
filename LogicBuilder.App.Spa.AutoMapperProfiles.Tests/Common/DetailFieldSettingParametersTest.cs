using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class DetailFieldSettingParametersTest
    {
        static DetailFieldSettingParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new DetailFieldSettingParameters(
                field: "FirstName",
                title: "First Name",
                type: "text",
                modelType: "MyApp.Domain.Entities.Student"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailFieldSettingDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.DetailItemType.Field, descriptor.DetailType);
            Assert.Equal("FirstName", descriptor.Field);
            Assert.Equal("First Name", descriptor.Title);
            Assert.Equal("text", descriptor.Type);
            Assert.Equal("MyApp.Domain.Entities.Student", descriptor.ModelType);
            Assert.Null(descriptor.FieldTemplate);
            Assert.Null(descriptor.ValueTextTemplate);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var fieldTemplate = new DetailFieldTemplateParameters("customTemplate");

            // Act
            var parameters = new DetailFieldSettingParameters(
                field: "Status",
                title: "Current Status",
                type: "text",
                modelType: "MyApp.Domain.Entities.Order",
                fieldTemplate: fieldTemplate,
                valueTextTemplate: null
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailFieldSettingDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.DetailItemType.Field, descriptor.DetailType);
            Assert.Equal("Status", descriptor.Field);
            Assert.Equal("Current Status", descriptor.Title);
            Assert.Equal("text", descriptor.Type);
            Assert.Equal("MyApp.Domain.Entities.Order", descriptor.ModelType);
            Assert.Equal(fieldTemplate.TemplateName, descriptor.FieldTemplate.TemplateName);
            Assert.Null(descriptor.ValueTextTemplate);
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
