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
    public class VariableDirectivesParametersTest
    {
        static VariableDirectivesParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var conditionalDirectives = new List<DirectiveParameters>
            {
                new(
                    new DirectiveDescriptionParameters("Directives", "hideIf"),
                    new ConditionGroupParameters("and")
                )
            };

            // Act
            var parameters = new VariableDirectivesParameters(
                field: "Status",
                conditionalDirectives: conditionalDirectives
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<VariableDirectivesDescriptor>(parameters);

            // Assert
            Assert.Equal("Status", descriptor.Field);
            Assert.Equal(conditionalDirectives[0].DirectiveDescription.FunctionName, descriptor.ConditionalDirectives[0].DirectiveDescription.FunctionName);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var conditionalDirectives = new List<DirectiveParameters>
            {
                new(
                    new DirectiveDescriptionParameters("Directives", "validateIf"),
                    new ConditionGroupParameters("or")
                )
            };

            var parameters = new VariableDirectivesParameters(
                field: "Priority",
                conditionalDirectives: conditionalDirectives,
                modelType: "MyApp.Domain.Entities.Task"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<VariableDirectivesDescriptor>(parameters);

            // Assert
            Assert.Equal("Priority", descriptor.Field);
            Assert.Equal(conditionalDirectives[0].DirectiveDescription.FunctionName, descriptor.ConditionalDirectives[0].DirectiveDescription.FunctionName);
            Assert.Equal("MyApp.Domain.Entities.Task", descriptor.ModelType);
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
