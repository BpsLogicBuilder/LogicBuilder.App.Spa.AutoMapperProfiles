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
    public class FilterGroupParametersTest
    {
        static FilterGroupParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithLogicOnly_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new FilterGroupParameters(logic: "and");
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FilterGroupDescriptor>(parameters);

            // Assert
            Assert.Equal("and", descriptor.Logic);
            Assert.Empty(descriptor.Filters!);
            Assert.Empty(descriptor.FilterGroups!);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var filters = new List<FilterDefinitionParameters>
            {
                new("Name", "eq", "John")
            };
            var filterGroups = new List<FilterGroupParameters>
            {
                new("or")
            };

            // Act
            var parameters = new FilterGroupParameters(
                logic: "and",
                filters: filters,
                filterGroups: filterGroups
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FilterGroupDescriptor>(parameters);

            // Assert
            Assert.Equal("and", descriptor.Logic);
            Assert.Equal(filters.Count, descriptor.Filters!.Count);
            Assert.Equal(filterGroups[0].Logic, descriptor.FilterGroups![0].Logic);
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
