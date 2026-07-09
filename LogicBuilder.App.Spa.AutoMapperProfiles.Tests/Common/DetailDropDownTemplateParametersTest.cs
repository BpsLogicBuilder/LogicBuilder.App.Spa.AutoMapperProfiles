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
    public class DetailDropDownTemplateParametersTest
    {
        static DetailDropDownTemplateParametersTest()
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
                "MyApp.Domain.Entities.Student",
                "MyApp.Data.Entities.Student",
                "System.Collections.Generic.IEnumerable`1",
                "System.Collections.Generic.IEnumerable`1"
            );

            // Act
            var parameters = new DetailDropDownTemplateParameters(
                templateName: "dropDownTemplate",
                placeHolderText: "Select...",
                textField: "Name",
                valueField: "Id",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailDropDownTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("dropDownTemplate", descriptor.TemplateName);
            Assert.Equal("Select...", descriptor.PlaceHolderText);
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
                "MyApp.Domain.Entities.Order",
                "MyApp.Data.Entities.Order",
                "System.Collections.Generic.List`1",
                "System.Collections.Generic.List`1"
            );

            // Act
            var parameters = new DetailDropDownTemplateParameters(
                templateName: "customDropDown",
                placeHolderText: "Choose...",
                textField: "FullName",
                valueField: "OrderId",
                textAndValueSelector: textAndValueSelector,
                requestDetails: requestDetails,
                reloadItemsFlowName: "ReloadOrdersFlow",
                modelType: "MyApp.Domain.Entities.Customer"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DetailDropDownTemplateDescriptor>(parameters);

            // Assert
            Assert.Equal("customDropDown", descriptor.TemplateName);
            Assert.Equal("Choose...", descriptor.PlaceHolderText);
            Assert.Equal("FullName", descriptor.TextField);
            Assert.Equal("OrderId", descriptor.ValueField);
            Assert.Equal(textAndValueSelector.ParameterName, descriptor.TextAndValueSelector.ParameterName);
            Assert.Equal(requestDetails.ModelType, descriptor.RequestDetails.ModelType);
            Assert.Equal("ReloadOrdersFlow", descriptor.ReloadItemsFlowName);
            Assert.Equal("MyApp.Domain.Entities.Customer", descriptor.ModelType);
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
