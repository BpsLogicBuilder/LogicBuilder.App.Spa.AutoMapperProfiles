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
    public class DetailListSettingsParametersTest
    {
        static DetailListSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var listTemplate = new DetailListTemplateParameters("listTemplate");
            var fieldSettings = new List<IDetailItemParameters>();

            // Act
            var parameters = new DetailListSettingsParameters(
                field: "Enrollments",
                title: "Student Enrollments",
                listTemplate: listTemplate,
                fieldSettings: fieldSettings
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailListSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.DetailItemType.List, descriptor.DetailType);
            Assert.Equal("Enrollments", descriptor.Field);
            Assert.Equal("Student Enrollments", descriptor.Title);
            Assert.Equal(listTemplate.TemplateName, descriptor.ListTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var listTemplate = new DetailListTemplateParameters("customList");
            var fieldSettings = new List<IDetailItemParameters>
            {
                new DetailFieldSettingParameters("CourseName", "Course", "text", "MyApp.Domain.Entities.Enrollment")
            };

            // Act
            var parameters = new DetailListSettingsParameters(
                field: "Courses",
                title: "Enrolled Courses",
                listTemplate: listTemplate,
                fieldSettings: fieldSettings,
                modelType: "MyApp.Domain.Entities.Student"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailListSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.DetailItemType.List, descriptor.DetailType);
            Assert.Equal("Courses", descriptor.Field);
            Assert.Equal("Enrolled Courses", descriptor.Title);
            Assert.Equal(listTemplate.TemplateName, descriptor.ListTemplate.TemplateName);
            Assert.Equal(((DetailFieldSettingParameters)fieldSettings[0]).Field, ((DetailFieldSettingDescriptor)descriptor.FieldSettings[0]).Field);
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
