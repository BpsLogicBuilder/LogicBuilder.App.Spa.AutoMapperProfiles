using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests.Common
{
    public class HtmlPageSettingsParametersTest
    {
        static HtmlPageSettingsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithDefaultValues_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new HtmlPageSettingsParameters();
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<HtmlPageSettingsDescriptor>(parameters);

            // Assert
            Assert.Null(descriptor.ContentTemplate);
            Assert.Null(descriptor.MessageTemplate);
        }

        [Fact]
        public void Constructor_WithContentTemplate_SetsPropertiesCorrectly()
        {
            // Arrange
            var contentTemplate = new ContentTemplateParameters("Title", "template");

            // Act
            var parameters = new HtmlPageSettingsParameters(
                contentTemplate: contentTemplate
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<HtmlPageSettingsDescriptor>(parameters);

            // Assert
            Assert.Equal(contentTemplate.Title, descriptor.ContentTemplate!.Title);
            Assert.Null(descriptor.MessageTemplate);
        }

        [Fact]
        public void Constructor_WithMessageTemplate_SetsPropertiesCorrectly()
        {
            // Arrange
            var messageTemplate = new MessageTemplateParameters("Caption", "Message", "template");

            // Act
            var parameters = new HtmlPageSettingsParameters(
                messageTemplate: messageTemplate
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<HtmlPageSettingsDescriptor>(parameters);

            // Assert
            Assert.Null(descriptor.ContentTemplate);
            Assert.Equal(messageTemplate.TemplateName, descriptor.MessageTemplate!.TemplateName);
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
