using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Forms.Parameters.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class MultiSelectFormControlSettingsParametersTest
    {
        static MultiSelectFormControlSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var keyFields = new List<string> { "CourseId" };
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Course",
                "MyApp.Data.Entities.Course",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.Generic.IEnumerable`1"
            );
            var multiSelectTemplate = new MultiSelectTemplateParameters(
                "multiSelectTemplate",
                "Select courses...",
                "CourseName",
                "CourseId",
                textAndValueSelector,
                requestDetails
            );

            // Act
            var parameters = new MultiSelectFormControlSettingsParameters(
                keyFields: keyFields,
                field: "Courses",
                domElementId: "courses_id",
                title: "Courses",
                placeHolder: "Select courses",
                type: "text",
                multiSelectTemplate: multiSelectTemplate,
                modelType: typeof(string).AssemblyQualifiedName!
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<MultiSelectFormControlSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.MultiSelectFormControl, descriptor.AbstractControlType);
            Assert.Equal(keyFields, descriptor.KeyFields);
            Assert.Equal("Courses", descriptor.Field);
            Assert.Equal("courses_id", descriptor.DomElementId);
            Assert.Equal("Courses", descriptor.Title);
            Assert.Equal("Select courses", descriptor.Placeholder);
            Assert.Equal("text", descriptor.Type);
            Assert.Equal(multiSelectTemplate.TemplateName, descriptor.MultiSelectTemplate.TemplateName);
            Assert.Equal(typeof(string).AssemblyQualifiedName!, descriptor.ModelType);
            Assert.Null(descriptor.ValidationSetting);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var keyFields = new List<string> { "TagId" };
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Tag",
                "MyApp.Data.Entities.Tag",
                "System.Collections.Generic.List`1",
                "System.Collections.Generic.List`1"
            );
            var multiSelectTemplate = new MultiSelectTemplateParameters(
                "tagMultiSelect",
                "Choose tags...",
                "TagName",
                "TagId",
                textAndValueSelector,
                requestDetails
            );
            var validationSetting = new FormValidationSettingParameters();

            // Act
            var parameters = new MultiSelectFormControlSettingsParameters(
                keyFields: keyFields,
                field: "Tags",
                domElementId: "tags_id",
                title: "Tags",
                placeHolder: "Choose tags",
                type: "text",
                multiSelectTemplate: multiSelectTemplate,
                validationSetting: validationSetting,
                modelType: "MyApp.Domain.Entities.Post"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<MultiSelectFormControlSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.MultiSelectFormControl, descriptor.AbstractControlType);
            Assert.Equal(keyFields, descriptor.KeyFields);
            Assert.Equal("Tags", descriptor.Field);
            Assert.Equal("tags_id", descriptor.DomElementId);
            Assert.Equal("Tags", descriptor.Title);
            Assert.Equal("Choose tags", descriptor.Placeholder);
            Assert.Equal("text", descriptor.Type);
            Assert.Equal(multiSelectTemplate.TemplateName, descriptor.MultiSelectTemplate.TemplateName);
            Assert.Equal(validationSetting.DefaultValue, descriptor.ValidationSetting!.DefaultValue);
            Assert.Equal("MyApp.Domain.Entities.Post", descriptor.ModelType);
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
