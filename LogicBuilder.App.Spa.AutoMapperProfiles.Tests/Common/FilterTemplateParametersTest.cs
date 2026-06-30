using AutoMapper;
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
    public class FilterTemplateParametersTest
    {
        static FilterTemplateParametersTest()
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
                "MyApp.Domain.Entities.Status",
                "MyApp.Data.Entities.Status",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.Generic.IEnumerable`1"
            );

            // Act
            var parameters = new FilterTemplateParameters(
                templateName: "filterTemplate",
                isPrimitive: true,
                textField: "Name",
                valueField: "Id",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FilterTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("filterTemplate", descriptor.TemplateName);
            Assert.True(descriptor.IsPrimitive);
            Assert.Equal("Name", descriptor.TextField);
            Assert.Equal("Id", descriptor.ValueField);
            Assert.Equal(textAndValueSelector.ParameterName, descriptor.TextAndValueSelector.ParameterName);
            Assert.Equal(requestDetails.DataSourceUrl, descriptor.RequestDetails.DataSourceUrl);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Category",
                "MyApp.Data.Entities.Category",
                "System.Collections.Generic.List`1",
                "System.Collections.Generic.List`1"
            );

            // Act
            var parameters = new FilterTemplateParameters(
                templateName: "categoryFilter",
                isPrimitive: false,
                textField: "CategoryName",
                valueField: "CategoryId",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails,
                modelType: "MyApp.Domain.Entities.Product"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FilterTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("categoryFilter", descriptor.TemplateName);
            Assert.False(descriptor.IsPrimitive);
            Assert.Equal("CategoryName", descriptor.TextField);
            Assert.Equal("CategoryId", descriptor.ValueField);
            Assert.Equal(textAndValueSelector.ParameterName, descriptor.TextAndValueSelector.ParameterName);
            Assert.Equal(requestDetails.ModelType, descriptor.RequestDetails.ModelType);
            Assert.Equal("MyApp.Domain.Entities.Product", descriptor.ModelType);
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
