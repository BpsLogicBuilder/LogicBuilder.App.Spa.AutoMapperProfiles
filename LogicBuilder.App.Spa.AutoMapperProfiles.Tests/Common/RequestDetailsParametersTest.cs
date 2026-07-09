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
    public class RequestDetailsParametersTest
    {
        static RequestDetailsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new RequestDetailsParameters(
                modelType: "MyApp.Domain.Entities.Student",
                dataType: "MyApp.Data.Entities.Student",
                modelReturnType: "System.Collections.Generic.IEnumerable`1",
                dataReturnType: "System.Collections.Generic.IEnumerable`1"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<RequestDetailsDescriptor>(parameters);

            // Assert
            Assert.Equal("MyApp.Domain.Entities.Student", descriptor.ModelType);
            Assert.Equal("MyApp.Data.Entities.Student", descriptor.DataType);
            Assert.Equal("System.Collections.Generic.IEnumerable`1", descriptor.ModelReturnType);
            Assert.Equal("System.Collections.Generic.IEnumerable`1", descriptor.DataReturnType);
            Assert.Equal("/api/List/GetList", descriptor.DataSourceUrl);
            Assert.Null(descriptor.SelectExpandDefinition);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new RequestDetailsParameters(
                modelType: "MyApp.Domain.Entities.Order",
                dataType: "MyApp.Data.Entities.Order",
                modelReturnType: "System.Collections.Generic.List`1",
                dataReturnType: "System.Collections.Generic.List`1",
                dataSourceUrl: "/api/Orders/GetList",
                selectExpandDefinition: null
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<RequestDetailsDescriptor>(parameters);

            // Assert
            Assert.Equal("MyApp.Domain.Entities.Order", descriptor.ModelType);
            Assert.Equal("MyApp.Data.Entities.Order", descriptor.DataType);
            Assert.Equal("System.Collections.Generic.List`1", descriptor.ModelReturnType);
            Assert.Equal("System.Collections.Generic.List`1", descriptor.DataReturnType);
            Assert.Equal("/api/Orders/GetList", descriptor.DataSourceUrl);
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
