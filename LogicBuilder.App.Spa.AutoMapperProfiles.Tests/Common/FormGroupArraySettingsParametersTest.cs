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
    public class FormGroupArraySettingsParametersTest
    {
        static FormGroupArraySettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var formGroupTemplate = new FormGroupTemplateParameters("arrayTemplate");
            var fieldSettings = new List<IFormItemSettingParameters>();
            var keyFields = new List<string> { "Id" };
            var validationMessages = new List<ValidationMessageParameters>();

            // Act
            var parameters = new FormGroupArraySettingsParameters(
                field: "Enrollments",
                formGroupTemplate: formGroupTemplate,
                fieldSettings: fieldSettings,
                keyFields: keyFields,
                validationMessages: validationMessages,
                title: "Student Enrollments",
                showTitle: true
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormGroupArraySettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.FormGroupArray, descriptor.AbstractControlType);
            Assert.Equal("Enrollments", descriptor.Field);
            Assert.Equal(formGroupTemplate.TemplateName, descriptor.FormGroupTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.NotNull(descriptor.ValidationMessages);
            Assert.Equal(keyFields, descriptor.KeyFields);
            Assert.NotNull(descriptor.ConditionalDirectives);
            Assert.Equal("Student Enrollments", descriptor.Title);
            Assert.True(descriptor.ShowTitle);
            Assert.Equal("<myApp>.Domain.Entities.<EntityName>", descriptor.ArrayElementType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var formGroupTemplate = new FormGroupTemplateParameters("orderItemsTemplate");
            var fieldSettings = new List<IFormItemSettingParameters>();
            var keyFields = new List<string> { "OrderItemId" };
            var validationMessages = new List<ValidationMessageParameters>
            {
                new("Quantity",
                [
                    new ValidationMethodParameters("required", "Quantity is required") 
                ])
            };
            var conditionalDirectives = new List<VariableDirectivesParameters>();

            // Act
            var parameters = new FormGroupArraySettingsParameters(
                field: "OrderItems",
                formGroupTemplate: formGroupTemplate,
                fieldSettings: fieldSettings,
                keyFields: keyFields,
                validationMessages: validationMessages,
                title: "Order Items",
                showTitle: false,
                arrayElementType: "MyApp.Domain.Entities.OrderItem",
                conditionalDirectives: conditionalDirectives
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormGroupArraySettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(Forms.Configuration.Common.AbstractControlType.FormGroupArray, descriptor.AbstractControlType);
            Assert.Equal("OrderItems", descriptor.Field);
            Assert.Equal(formGroupTemplate.TemplateName, descriptor.FormGroupTemplate.TemplateName);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.NotNull(descriptor.ValidationMessages);
            Assert.Equal(keyFields, descriptor.KeyFields);
            Assert.NotNull(descriptor.ConditionalDirectives);
            Assert.Equal("Order Items", descriptor.Title);
            Assert.False(descriptor.ShowTitle);
            Assert.Equal("MyApp.Domain.Entities.OrderItem", descriptor.ArrayElementType);
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
