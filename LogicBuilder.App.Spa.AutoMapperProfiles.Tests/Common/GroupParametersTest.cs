using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class GroupParametersTest
    {
        static GroupParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var aggregates = new List<AggregateDefinitionParameters>();

            // Act
            var parameters = new GroupParameters(
                field: "Category",
                dir: "asc",
                aggregates: aggregates
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<GroupDescriptor>(parameters);

            // Assert
            Assert.Equal("Category", descriptor.Field);
            Assert.Equal("asc", descriptor.Dir);
            Assert.Equal(aggregates.Count, descriptor.Aggregates.Count);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var aggregates = new List<AggregateDefinitionParameters>
            {
                new("Count", "count")
            };

            // Act
            var parameters = new GroupParameters(
                field: "Status",
                dir: "desc",
                aggregates: aggregates,
                modelType: "MyApp.Domain.Entities.Order"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<GroupDescriptor>(parameters);

            // Assert
            Assert.Equal("Status", descriptor.Field);
            Assert.Equal("desc", descriptor.Dir);
            Assert.Equal(aggregates[0].Field, descriptor.Aggregates[0].Field);
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
