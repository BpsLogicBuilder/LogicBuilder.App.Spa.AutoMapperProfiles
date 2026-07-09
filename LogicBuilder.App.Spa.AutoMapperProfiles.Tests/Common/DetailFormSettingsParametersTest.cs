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
    public class DetailFormSettingsParametersTest
    {
        static DetailFormSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var requestDetails = new FormRequestDetailsParameters(
                getUrl: "/api/Entity/Get",
                addUrl: "/api/Entity/Add",
                updateUrl: "/api/Entity/Update",
                deleteUrl: "/api/Entity/Delete",
                modelType: "MyApp.Domain.Entities.Entity",
                dataType: "MyApp.Data.Entities.Entity"
            );
            var fieldSettings = new List<IDetailItemParameters>();

            // Act
            var parameters = new DetailFormSettingsParameters(
                title: "Entity Details",
                displayField: "Name",
                requestDetails: requestDetails,
                fieldSettings: fieldSettings
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailFormSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Entity Details", descriptor.Title);
            Assert.Equal("Name", descriptor.DisplayField);
            Assert.Equal(requestDetails.DeleteUrl, descriptor.RequestDetails.DeleteUrl);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.Equal("<myApp>.Domain.Entities.<EntityName>", descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var requestDetails = new FormRequestDetailsParameters(
                getUrl: "/api/Student/Get",
                addUrl: "/api/Student/Add",
                updateUrl: "/api/Student/Update",
                deleteUrl: "/api/Student/Delete",
                modelType: "MyApp.Domain.Entities.Student",
                dataType: "MyApp.Data.Entities.Student"
            );
            var fieldSettings = new List<IDetailItemParameters>
            {
                new DetailFieldSettingParameters("FirstName", "First Name", "text", "MyApp.Domain.Entities.Student")
            };

            // Act
            var parameters = new DetailFormSettingsParameters(
                title: "Student Details",
                displayField: "FullName",
                requestDetails: requestDetails,
                fieldSettings: fieldSettings,
                modelType: "MyApp.Domain.Entities.Student"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailFormSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Student Details", descriptor.Title);
            Assert.Equal("FullName", descriptor.DisplayField);
            Assert.Equal(requestDetails.GetUrl, descriptor.RequestDetails.GetUrl);
            Assert.Equal(((DetailFieldSettingParameters)fieldSettings[0]).Title, ((DetailFieldSettingDescriptor)descriptor.FieldSettings[0]).Title);
            Assert.Equal("MyApp.Domain.Entities.Student", descriptor.ModelType);
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
