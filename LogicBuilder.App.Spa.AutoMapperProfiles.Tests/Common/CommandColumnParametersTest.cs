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
    public class CommandColumnParametersTest
    {
        static CommandColumnParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new CommandColumnParameters(
                title: "Actions",
                width: 150
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandColumnDescriptor>(parameters);

            // Assert
            Assert.Equal("Actions", descriptor.Title);
            Assert.Equal(150, descriptor.Width);
        }

        [Fact]
        public void Constructor_WithNullWidth_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new CommandColumnParameters(
                title: "Commands",
                width: null
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandColumnDescriptor>(parameters);

            // Assert
            Assert.Equal("Commands", descriptor.Title);
            Assert.Null(descriptor.Width);
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
