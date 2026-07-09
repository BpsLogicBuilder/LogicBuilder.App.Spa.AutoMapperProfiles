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
    public class DataRequestStateParametersTest
    {
        static DataRequestStateParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithDefaultValues_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new DataRequestStateParameters();
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DataRequestStateDescriptor>(parameters);

            // Assert
            Assert.Null(descriptor.Skip);
            Assert.Null(descriptor.Take);
            Assert.Empty(descriptor.Sort!);
            Assert.Empty(descriptor.Group!);
            Assert.Null(descriptor.FilterGroup);
            Assert.Empty(descriptor.Aggregates!);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var sort = new List<SortParameters> { new("Name", "asc") };
            var group = new List<GroupParameters> { new("Category", "asc", []) };
            var filterGroup = new FilterGroupParameters("and");
            var aggregates = new List<AggregateDefinitionParameters> { new("Count", "count") };

            // Act
            var parameters = new DataRequestStateParameters(
                skip: 10,
                take: 20,
                sort: sort,
                group: group,
                filterGroup: filterGroup,
                aggregates: aggregates
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DataRequestStateDescriptor>(parameters);

            // Assert
            Assert.Equal(10, descriptor.Skip);
            Assert.Equal(20, descriptor.Take);
            Assert.Equal(sort[0].Dir, descriptor.Sort![0].Dir);
            Assert.Equal(group[0].Field, descriptor.Group![0].Field);
            Assert.Equal(filterGroup.Logic, descriptor.FilterGroup!.Logic);
            Assert.Equal(aggregates[0].Field, descriptor.Aggregates![0].Field);
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
