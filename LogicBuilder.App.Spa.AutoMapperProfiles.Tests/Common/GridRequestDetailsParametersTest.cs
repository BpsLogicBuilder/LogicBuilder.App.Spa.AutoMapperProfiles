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
    public class GridRequestDetailsParametersTest
    {
        static GridRequestDetailsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new GridRequestDetailsParameters(
                modelType: "MyApp.Domain.Entities.Student",
                dataType: "MyApp.Data.Entities.Student"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<GridRequestDetailsDescriptor>(parameters);

            // Assert
            Assert.Equal("MyApp.Domain.Entities.Student", descriptor.ModelType);
            Assert.Equal("MyApp.Data.Entities.Student", descriptor.DataType);
            Assert.Equal("/api/Generic/GetData", descriptor.DataSourceUrl);
            Assert.Null(descriptor.SelectExpandDefinition);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new GridRequestDetailsParameters(
                modelType: "MyApp.Domain.Entities.Order",
                dataType: "MyApp.Data.Entities.Order",
                dataSourceUrl: "/api/Orders/GetData",
                selectExpandDefinition: null
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<GridRequestDetailsDescriptor>(parameters);

            // Assert
            Assert.Equal("MyApp.Domain.Entities.Order", descriptor.ModelType);
            Assert.Equal("MyApp.Data.Entities.Order", descriptor.DataType);
            Assert.Equal("/api/Orders/GetData", descriptor.DataSourceUrl);
            Assert.Null(descriptor.SelectExpandDefinition);
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
