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
    public class GridSettingsParametersTest
    {
        static GridSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var requestDetails = new GridRequestDetailsParameters(
                modelType: "MyApp.Domain.Entities.Student",
                dataType: "MyApp.Data.Entities.Student"
            );
            var columns = new List<ColumnSettingsParameters>
            {
                new("Name", "Student Name", "text")
            };

            // Act
            var parameters = new GridSettingsParameters(
                title: "Students Grid",
                sortable: true,
                pageable: true,
                scrollable: "scrollable",
                groupable: false,
                isFilterable: true,
                filterableType: "menu",
                requestDetails: requestDetails,
                columns: columns
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<GridSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Students Grid", descriptor.Title);
            Assert.True(descriptor.Sortable);
            Assert.True(descriptor.Pageable);
            Assert.Equal("scrollable", descriptor.Scrollable);
            Assert.False(descriptor.Groupable);
            Assert.True(descriptor.IsFilterable);
            Assert.Equal("menu", descriptor.FilterableType);
            Assert.Equal(requestDetails.ModelType, descriptor.RequestDetails!.ModelType);
            Assert.Equal(columns[0].ModelType, descriptor.Columns[0].ModelType);
            Assert.Null(descriptor.GridId);
            Assert.Null(descriptor.Height);
            Assert.Null(descriptor.CommandColumn);
            Assert.Null(descriptor.State);
            Assert.Empty(descriptor.Aggregates!);
            Assert.Null(descriptor.DetailGridSettings);
        }

        [Fact]
        public void Constructor_FilterableProperty_ReturnsCorrectValue()
        {
            // Arrange
            var requestDetails = new GridRequestDetailsParameters(
                modelType: "MyApp.Domain.Entities.Student",
                dataType: "MyApp.Data.Entities.Student"
            );
            var columns = new List<ColumnSettingsParameters>();

            // Act - with filterableType set
            var parameters1 = new GridSettingsParameters(
                title: "Test Grid",
                sortable: true,
                pageable: true,
                scrollable: "",
                groupable: false,
                isFilterable: false,
                filterableType: "row",
                requestDetails: requestDetails,
                columns: columns
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor1 = mapper.Map<GridSettingsDescriptor>(parameters1);

            // Act - without filterableType
            var parameters2 = new GridSettingsParameters(
                title: "Test Grid",
                sortable: true,
                pageable: true,
                scrollable: "",
                groupable: false,
                isFilterable: true,
                filterableType: "",
                requestDetails: requestDetails,
                columns: columns
            );

            //ACt
            var descriptor2 = mapper.Map<GridSettingsDescriptor>(parameters2);

            // Assert
            Assert.Equal("row", descriptor1.Filterable);
            Assert.True((bool)descriptor2.Filterable);
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
