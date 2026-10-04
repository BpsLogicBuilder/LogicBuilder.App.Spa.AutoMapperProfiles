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
    public class ChatFormSettingsParametersTest
    {
        static ChatFormSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Map_ChatFormSettingsParameters_ToDescriptor_SetsPropertiesCorrectly()
        {
            // Arrange
            var signalR = new SignalRConnectionParameters(
                "/agentChatHub",
                "ReceiveAgentError",
                "ReceiveAgentChunk",
                "ReceiveAgentResponseComplete",
                "SendMessageToAgent",
                "SessionInitialized"
            );
            var parameters = new ChatFormSettingsParameters("Agent Chat", "knowledge-search-only", 550, 600, signalR);
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            // Act
            var descriptor = mapper.Map<ChatFormSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal("Agent Chat", descriptor.Title);
            Assert.Equal("knowledge-search-only", descriptor.AgentConfigurationIdentifier);
            Assert.Equal(550, descriptor.ChatHeight);
            Assert.Equal(600, descriptor.ChatWidth);
            Assert.NotNull(descriptor.SignalRConnection);
            Assert.Equal("/agentChatHub", descriptor.SignalRConnection.AgentHubUrl);
            Assert.Equal("SessionInitialized", descriptor.SignalRConnection.SessionInitializedHandler);
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
