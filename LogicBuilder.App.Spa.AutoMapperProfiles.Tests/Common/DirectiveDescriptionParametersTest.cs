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
    public class DirectiveDescriptionParametersTest
    {
        static DirectiveDescriptionParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new DirectiveDescriptionParameters(
                className: "Directives",
                functionName: "hideIf"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DirectiveDescriptionDescriptor>(parameters);

            // Assert
            Assert.Equal("Directives", descriptor.ClassName);
            Assert.Equal("hideIf", descriptor.FunctionName);
            Assert.Empty(descriptor.Arguments);
        }

        [Fact]
        public void Constructor_WithArguments_SetsPropertiesCorrectly()
        {
            // Arrange
            var arguments = new List<DirectiveArgumentParameters>
            {
                new("arg1", "value1"),
                new("arg2", 42)
            };

            // Act
            var parameters = new DirectiveDescriptionParameters(
                className: "CustomDirectives",
                functionName: "validateIf",
                arguments: arguments
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<DirectiveDescriptionDescriptor>(parameters);

            // Assert
            Assert.Equal("CustomDirectives", descriptor.ClassName);
            Assert.Equal("validateIf", descriptor.FunctionName);
            Assert.NotNull(descriptor.Arguments);
            Assert.Equal(2, descriptor.Arguments.Count);
            Assert.Equal("value1", descriptor.Arguments["arg1"]);
            Assert.Equal(42, descriptor.Arguments["arg2"]);
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
