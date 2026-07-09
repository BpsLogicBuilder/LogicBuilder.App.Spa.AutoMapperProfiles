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
    public class DropDownTemplateParametersTest
    {
        static DropDownTemplateParametersTest()
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
                "MyApp.Domain.Entities.Category",
                "MyApp.Data.Entities.Category",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.Generic.IEnumerable`1"
            );

            // Act
            var parameters = new DropDownTemplateParameters(
                templateName: "dropDownTemplate",
                placeHolderText: "Select One...",
                textField: "Name",
                valueField: "Id",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DropDownTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("dropDownTemplate", descriptor.TemplateName);
            Assert.Equal("Select One...", descriptor.PlaceHolderText);
            Assert.Equal("Name", descriptor.TextField);
            Assert.Equal("Id", descriptor.ValueField);
            Assert.Equal(textAndValueSelector.ParameterName, descriptor.TextAndValueSelector.ParameterName);
            Assert.Equal(requestDetails.ModelType, descriptor.RequestDetails.ModelType);
            Assert.Null(descriptor.ReloadItemsFlowName);
            Assert.Equal("<myApp>.Domain.Entities.<EntityName>", descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var textAndValueSelector = new SelectorLambdaOperatorParameters(null!, null!, null!);
            var requestDetails = new RequestDetailsParameters(
                "MyApp.Domain.Entities.Product",
                "MyApp.Data.Entities.Product",
                "System.Collections.Generic.List`1",
                "System.Collections.Generic.List`1"
            );

            // Act
            var parameters = new DropDownTemplateParameters(
                templateName: "productDropDown",
                placeHolderText: "Choose Product...",
                textField: "ProductName",
                valueField: "ProductId",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails,
                reloadItemsFlowName: "ReloadProductsFlow",
                modelType: "MyApp.Domain.Entities.Product"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DropDownTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("productDropDown", descriptor.TemplateName);
            Assert.Equal("Choose Product...", descriptor.PlaceHolderText);
            Assert.Equal("ProductName", descriptor.TextField);
            Assert.Equal("ProductId", descriptor.ValueField);
            Assert.Equal(textAndValueSelector.ParameterName, descriptor.TextAndValueSelector.ParameterName);
            Assert.Equal(requestDetails.ModelType, descriptor.RequestDetails.ModelType);
            Assert.Equal("ReloadProductsFlow", descriptor.ReloadItemsFlowName);
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
