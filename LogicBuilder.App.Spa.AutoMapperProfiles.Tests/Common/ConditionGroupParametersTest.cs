using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.EntityFrameworkCore.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class ConditionGroupParametersTest
    {
        static ConditionGroupParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithLogicOnly_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new ConditionGroupParameters(logic: "and");
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ConditionGroupDescriptor>(parameters);

            // Assert
            Assert.Equal("and", descriptor.Logic);
            Assert.Empty(descriptor.Conditions!);
            Assert.Empty(descriptor.ConditionGroups!);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var conditions = new List<ConditionParameters>
            {
                new(@operator: "eq", leftVariable: "Status")
            };
            var conditionGroups = new List<ConditionGroupParameters>
            {
                new(logic: "or")
            };

            // Act
            var parameters = new ConditionGroupParameters(
                logic: "and",
                conditions: conditions,
                conditionGroups: conditionGroups
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ConditionGroupDescriptor>(parameters);

            // Assert
            Assert.Equal("and", descriptor.Logic);
            Assert.Equal(conditions[0].Operator, descriptor.Conditions![0].Operator);
            Assert.Equal(conditionGroups[0].Logic, descriptor.ConditionGroups![0].Logic);
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
