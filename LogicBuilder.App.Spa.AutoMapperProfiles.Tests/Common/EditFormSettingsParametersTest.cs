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
    public class EditFormSettingsParametersTest
    {
        static EditFormSettingsParametersTest()
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
                "/api/Student/Get",
                "/api/Student/Add",
                "/api/Student/Update",
                "/api/Student/Delete",
                "MyApp.Domain.Entities.Student",
                "MyApp.Data.Entities.Student"
            );
            var validationMessages = new List<ValidationMessageParameters>();
            var fieldSettings = new List<IFormItemSettingParameters>();

            // Act
            var parameters = new EditFormSettingsParameters(
                title: "Student Form",
                displayField: "FullName",
                requestDetails: requestDetails,
                validationMessages: validationMessages,
                fieldSettings: fieldSettings
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<EditFormSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Student Form", descriptor.Title);
            Assert.Equal("FullName", descriptor.DisplayField);
            Assert.Equal(requestDetails.GetUrl, descriptor.RequestDetails.GetUrl);
            Assert.NotNull(descriptor.ValidationMessages);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.NotNull(descriptor.ConditionalDirectives);
            Assert.Equal("Contoso.Domain.Entities.XXXX , Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var requestDetails = new FormRequestDetailsParameters(
                "/api/Order/Get",
                "/api/Order/Add",
                "/api/Order/Update",
                "/api/Order/Delete",
                "MyApp.Domain.Entities.Order",
                "MyApp.Data.Entities.Order"
            );
            var validationMessages = new List<ValidationMessageParameters>
            {
                new("Status",
                [
                    new ValidationMethodParameters("required", "Status is required") 
                ])
            };
            var fieldSettings = new List<IFormItemSettingParameters>();
            var conditionalDirectives = new List<VariableDirectivesParameters>
            {
                new("Status", [])
            };

            // Act
            var parameters = new EditFormSettingsParameters(
                title: "Order Form",
                displayField: "OrderNumber",
                requestDetails: requestDetails,
                validationMessages: validationMessages,
                fieldSettings: fieldSettings,
                conditionalDirectives: conditionalDirectives,
                modelType: "MyApp.Domain.Entities.Order, MyApp.Domain"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<EditFormSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Order Form", descriptor.Title);
            Assert.Equal("OrderNumber", descriptor.DisplayField);
            Assert.Equal(requestDetails.GetUrl, descriptor.RequestDetails.GetUrl);
            Assert.NotNull(descriptor.ValidationMessages);
            Assert.Single(descriptor.ValidationMessages);
            Assert.Equal(fieldSettings.Count, descriptor.FieldSettings.Count);
            Assert.NotNull(descriptor.ConditionalDirectives);
            Assert.Equal("MyApp.Domain.Entities.Order, MyApp.Domain", descriptor.ModelType);
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
