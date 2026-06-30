using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Forms.Parameters.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class MultiSelectTemplateParametersTest
    {
        static MultiSelectTemplateParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Role",
                "MyApp.Data.Entities.Role",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.Generic.IEnumerable`1"
            );

            // Act
            var parameters = new MultiSelectTemplateParameters(
                templateName: "multiSelectTemplate",
                placeHolderText: "Select roles...",
                textField: "RoleName",
                valueField: "RoleId",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<MultiSelectTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("multiSelectTemplate", descriptor.TemplateName);
            Assert.Equal("Select roles...", descriptor.PlaceHolderText);
            Assert.Equal("RoleName", descriptor.TextField);
            Assert.Equal("RoleId", descriptor.ValueField);
            Assert.Equal(textAndValueSelector.ParameterName, descriptor.TextAndValueSelector.ParameterName);
            Assert.Equal(requestDetails.DataType, descriptor.RequestDetails.DataType);
            Assert.Equal("<myApp>.Domain.Entities.<EntityName>", descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Permission",
                "MyApp.Data.Entities.Permission",
                "System.Collections.Generic.List`1",
                "System.Collections.Generic.List`1"
            );

            // Act
            var parameters = new MultiSelectTemplateParameters(
                templateName: "permissionMultiSelect",
                placeHolderText: "Choose permissions...",
                textField: "PermissionName",
                valueField: "PermissionId",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails,
                modelType: "MyApp.Domain.Entities.User"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<MultiSelectTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("permissionMultiSelect", descriptor.TemplateName);
            Assert.Equal("Choose permissions...", descriptor.PlaceHolderText);
            Assert.Equal("PermissionName", descriptor.TextField);
            Assert.Equal("PermissionId", descriptor.ValueField);
            Assert.Equal(textAndValueSelector.ParameterName, descriptor.TextAndValueSelector.ParameterName);
            Assert.Equal(requestDetails.DataSourceUrl, descriptor.RequestDetails.DataSourceUrl);
            Assert.Equal("MyApp.Domain.Entities.User", descriptor.ModelType);
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
