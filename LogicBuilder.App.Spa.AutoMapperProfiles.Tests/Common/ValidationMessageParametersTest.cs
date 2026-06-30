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
    public class ValidationMessageParametersTest
    {
        static ValidationMessageParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var methods = new List<ValidationMethodParameters>
            {
                new("required", "Field is required")
            };
            var parameters = new ValidationMessageParameters(
                field: "FirstName",
                methods: methods
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ValidationMessageDescriptor>(parameters);

            // Assert
            Assert.Equal("FirstName", descriptor.Field);
            Assert.NotNull(descriptor.Methods);
            Assert.Single(descriptor.Methods);
            Assert.Equal("Field is required", descriptor.Methods["required"]);
            Assert.Null(descriptor.ModelType);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var methods = new List<ValidationMethodParameters>
            {
                new("required", "Name is required"),
                new("maxLength", "Name is too long")
            };
            var parameters = new ValidationMessageParameters(
                field: "LastName",
                methods: methods,
                modelType: "MyApp.Domain.Entities.Person"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<ValidationMessageDescriptor>(parameters);

            // Assert
            Assert.Equal("LastName", descriptor.Field);
            Assert.Equal(2, descriptor.Methods.Count);
            Assert.Equal("Name is required", descriptor.Methods["required"]);
            Assert.Equal("Name is too long", descriptor.Methods["maxLength"]);
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
