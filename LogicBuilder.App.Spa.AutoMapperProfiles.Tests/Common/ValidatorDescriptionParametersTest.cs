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
    public class ValidatorDescriptionParametersTest
    {
        static ValidatorDescriptionParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new ValidatorDescriptionParameters(
                className: "Validators",
                functionName: "required"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ValidatorDescriptionDescriptor>(parameters);

            // Assert
            Assert.Equal("Validators", descriptor.ClassName);
            Assert.Equal("required", descriptor.FunctionName);
            Assert.Empty(descriptor.Arguments!);
        }

        [Fact]
        public void Constructor_WithArguments_SetsPropertiesCorrectly()
        {
            // Arrange
            var arguments = new List<ValidatorArgumentParameters>
            {
                new("min", 5),
                new("max", 100)
            };

            var parameters = new ValidatorDescriptionParameters(
                className: "CustomValidators",
                functionName: "range",
                arguments: arguments
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ValidatorDescriptionDescriptor>(parameters);

            // Assert
            Assert.Equal("CustomValidators", descriptor.ClassName);
            Assert.Equal("range", descriptor.FunctionName);
            Assert.NotNull(descriptor.Arguments);
            Assert.Equal(2, descriptor.Arguments.Count);
            Assert.Equal(5, descriptor.Arguments["min"]);
            Assert.Equal(100, descriptor.Arguments["max"]);
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
