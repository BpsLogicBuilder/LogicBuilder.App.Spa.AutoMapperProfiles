using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class FilterDefinitionParametersTest
    {
        static FilterDefinitionParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new FilterDefinitionParameters(
                field: "Name",
                oper: "eq"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FilterDefinitionDescriptor>(parameters);

            // Assert
            Assert.Equal("Name", descriptor.Field);
            Assert.Equal("eq", descriptor.Operator);
            Assert.Null(descriptor.Value);
            Assert.Null(descriptor.IgnoreCase);
            Assert.Equal("", descriptor.ValueSourceMember);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new FilterDefinitionParameters(
                field: "LastName",
                oper: "contains",
                value: "Smith",
                ignoreCase: true,
                valueSourceMember: "SearchValue",
                modelType: "MyApp.Domain.Entities.Person"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FilterDefinitionDescriptor>(parameters);

            // Assert
            Assert.Equal("LastName", descriptor.Field);
            Assert.Equal("contains", descriptor.Operator);
            Assert.Equal("Smith", descriptor.Value);
            Assert.True(descriptor.IgnoreCase);
            Assert.Equal("SearchValue", descriptor.ValueSourceMember);
            Assert.Equal("MyApp.Domain.Entities.Person", descriptor.ModelType);
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
