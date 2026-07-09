using AutoMapper;
using LogicBuilder.App.Spa.Forms.Configuration;
using LogicBuilder.App.Spa.Forms.Parameters;
using LogicBuilder.EntityFrameworkCore.Mapping;
using LogicBuilder.Forms.Parameters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.AutoMapperProfiles.Tests
{
    public class CommandButtonParametersTest
    {
        static CommandButtonParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithDefaultValues_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters();
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.False(descriptor.Cancel);
            Assert.Null(descriptor.GridId);
            Assert.Null(descriptor.GridCommandButton);
            Assert.Equal("fa-step-forward", descriptor.ButtonIcon);
            Assert.Equal("btn btn-secondary", descriptor.ClassString);
        }

        [Fact]
        public void Constructor_WithCustomCancel_SetsPropertyCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters(cancel: true);
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.True(descriptor.Cancel);
            Assert.Null(descriptor.GridId);
            Assert.Null(descriptor.GridCommandButton);
            Assert.Equal("fa-step-forward", descriptor.ButtonIcon);
            Assert.Equal("btn btn-secondary", descriptor.ClassString);
        }

        [Fact]
        public void Constructor_WithCustomGridId_SetsPropertyCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters(gridId: 42);
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.False(descriptor.Cancel);
            Assert.Equal(42, descriptor.GridId);
            Assert.Null(descriptor.GridCommandButton);
            Assert.Equal("fa-step-forward", descriptor.ButtonIcon);
            Assert.Equal("btn btn-secondary", descriptor.ClassString);
        }

        [Fact]
        public void Constructor_WithCustomGridCommandButton_SetsPropertyCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters(gridCommandButton: true);
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.False(descriptor.Cancel);
            Assert.Null(descriptor.GridId);
            Assert.True(descriptor.GridCommandButton);
            Assert.Equal("fa-step-forward", descriptor.ButtonIcon);
            Assert.Equal("btn btn-secondary", descriptor.ClassString);
        }

        [Fact]
        public void Constructor_WithCustomButtonIcon_SetsPropertyCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters(buttonIcon: "fa-save");
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.False(descriptor.Cancel);
            Assert.Null(descriptor.GridId);
            Assert.Null(descriptor.GridCommandButton);
            Assert.Equal("fa-save", descriptor.ButtonIcon);
            Assert.Equal("btn btn-secondary", descriptor.ClassString);
        }

        [Fact]
        public void Constructor_WithCustomClassString_SetsPropertyCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters(classString: "btn btn-primary");
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.False(descriptor.Cancel);
            Assert.Null(descriptor.GridId);
            Assert.Null(descriptor.GridCommandButton);
            Assert.Equal("fa-step-forward", descriptor.ButtonIcon);
            Assert.Equal("btn btn-primary", descriptor.ClassString);
        }

        [Fact]
        public void Constructor_WithAllCustomValues_SetsAllPropertiesCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters(
                cancel: true,
                gridId: 100,
                gridCommandButton: false,
                buttonIcon: "fa-trash",
                classString: "btn btn-danger"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.True(descriptor.Cancel);
            Assert.Equal(100, descriptor.GridId);
            Assert.False(descriptor.GridCommandButton);
            Assert.Equal("fa-trash", descriptor.ButtonIcon);
            Assert.Equal("btn btn-danger", descriptor.ClassString);
        }

        [Fact]
        public void Constructor_WithMixedValues_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new CommandButtonParameters(
                cancel: true,
                buttonIcon: "fa-edit",
                classString: "btn btn-warning"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<CommandButtonDescriptor>(parameters);

            // Assert
            Assert.True(descriptor.Cancel);
            Assert.Null(descriptor.GridId);
            Assert.Null(descriptor.GridCommandButton);
            Assert.Equal("fa-edit", descriptor.ButtonIcon);
            Assert.Equal("btn btn-warning", descriptor.ClassString);
        }

        [Fact]
        public void Map_ConnectorParameters_To_CommandButtonDescriptor()
        {
            //Arrange
            ConnectorParameters parameters = new()
            {
                Id = 1,
                ShortString = "EDT",
                LongString = "Edit",
                ConnectorData = new CommandButtonParameters
                (
                    buttonIcon: "fa-step-forward",
                    cancel: false,
                    classString: "btn-secondary",
                    gridCommandButton: true,
                    gridId: 1
                )
            };
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //Act
            CommandButtonDescriptor button = mapper.Map<CommandButtonDescriptor>(parameters);

            //Assert
            Assert.Equal(1, button.Id);
            Assert.Equal("EDT", button.ShortString);
            Assert.Equal("Edit", button.LongString);
            Assert.Equal("fa-step-forward", button.ButtonIcon);
            Assert.False(button.Cancel);
            Assert.Equal("btn-secondary", button.ClassString);
            Assert.True(button.GridCommandButton);
            Assert.Equal(1, button.GridId);
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
