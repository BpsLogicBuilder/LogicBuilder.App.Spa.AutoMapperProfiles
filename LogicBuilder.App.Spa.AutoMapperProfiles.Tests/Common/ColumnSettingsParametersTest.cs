using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.EntityFrameworkCore.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class ColumnSettingsParametersTest
    {
        static ColumnSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new ColumnSettingsParameters(
                field: "Name",
                title: "Full Name",
                type: "text"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ColumnSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Name", descriptor.Field);
            Assert.Equal("Full Name", descriptor.Title);
            Assert.Equal("text", descriptor.Type);
            Assert.Null(descriptor.Groupable);
            Assert.Null(descriptor.Width);
            Assert.Null(descriptor.Format);
            Assert.Null(descriptor.Filter);
            Assert.Null(descriptor.CellTemplate);
            Assert.Null(descriptor.CellListTemplate);
            Assert.Null(descriptor.FilterRowTemplate);
            Assert.Null(descriptor.FilterMenuTemplate);
            Assert.Null(descriptor.GroupHeaderTemplate);
            Assert.Null(descriptor.GroupFooterTemplate);
            Assert.Null(descriptor.GridFooterTemplate);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var cellTemplate = new CellTemplateParameters("customCell");
            var cellListTemplate = new CellListTemplateParameters("listTemplate", "DisplayName");

            // Act
            var parameters = new ColumnSettingsParameters(
                field: "Status",
                title: "Order Status",
                type: "text",
                groupable: true,
                width: 200,
                format: "{0:C}",
                filter: "text",
                cellTemplate: cellTemplate,
                cellListTemplate: cellListTemplate,
                filterRowTemplate: null,
                filterMenuTemplate: null,
                groupHeaderTemplate: null,
                groupFooterTemplate: null,
                gridFooterTemplate: null,
                modelType: "MyApp.Domain.Entities.Order"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ColumnSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Status", descriptor.Field);
            Assert.Equal("Order Status", descriptor.Title);
            Assert.Equal("text", descriptor.Type);
            Assert.True(descriptor.Groupable);
            Assert.Equal(200, descriptor.Width);
            Assert.Equal("{0:C}", descriptor.Format);
            Assert.Equal("text", descriptor.Filter);
            Assert.Equal("customCell", descriptor.CellTemplate!.TemplateName);
            Assert.Equal("listTemplate", descriptor.CellListTemplate!.TemplateName);
            Assert.Equal("MyApp.Domain.Entities.Order", descriptor.ModelType);
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
