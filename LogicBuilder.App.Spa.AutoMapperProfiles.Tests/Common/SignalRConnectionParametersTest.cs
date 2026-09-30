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
    public class SignalRConnectionParametersTest
    {
        static SignalRConnectionParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Map_SignalRConnectionParameters_ToDescriptor_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new SignalRConnectionParameters(
                "/agentChatHub",
                "ReceiveAgentError",
                "ReceiveAgentChunk",
                "ReceiveAgentResponseComplete",
                "SendMessageToAgent",
                "SessionInitialized"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            // Act
            var descriptor = mapper.Map<SignalRConnectionDescriptor>(parameters);

            // Assert
            Assert.Equal("/agentChatHub", descriptor.AgentHubUrl);
            Assert.Equal("ReceiveAgentError", descriptor.ReceiveAgentErrorHandler);
            Assert.Equal("ReceiveAgentChunk", descriptor.ReceiveAgentMessageHandler);
            Assert.Equal("ReceiveAgentResponseComplete", descriptor.ReceiveAgentResponseCompleteHandler);
            Assert.Equal("SendMessageToAgent", descriptor.SendMessageToAgentHubMethodName);
            Assert.Equal("SessionInitialized", descriptor.SessionInitializedHandler);
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
