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
    public class FormRequestDetailsParametersTest
    {
        static FormRequestDetailsParametersTest()
        {
            Initialize();
        }

        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;

        [Fact]
        public void Constructor_WithRequiredParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new FormRequestDetailsParameters(
                getUrl: "/api/Student/Get",
                addUrl: "/api/Student/Add",
                updateUrl: "/api/Student/Update",
                deleteUrl: "/api/Student/Delete",
                modelType: "MyApp.Domain.Entities.Student",
                dataType: "MyApp.Data.Entities.Student"
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormRequestDetailsDescriptor>(parameters);

            // Assert
            Assert.Equal("/api/Student/Get", descriptor.GetUrl);
            Assert.Equal("/api/Student/Add", descriptor.AddUrl);
            Assert.Equal("/api/Student/Update", descriptor.UpdateUrl);
            Assert.Equal("/api/Student/Delete", descriptor.DeleteUrl);
            Assert.Equal("MyApp.Domain.Entities.Student", descriptor.ModelType);
            Assert.Equal("MyApp.Data.Entities.Student", descriptor.DataType);
            Assert.Null(descriptor.Filter);
            Assert.Null(descriptor.SelectExpandDefinition);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var parameters = new FormRequestDetailsParameters(
                getUrl: "/api/Order/Get",
                addUrl: "/api/Order/Add",
                updateUrl: "/api/Order/Update",
                deleteUrl: "/api/Order/Delete",
                modelType: "MyApp.Domain.Entities.Order",
                dataType: "MyApp.Data.Entities.Order",
                filter: null,
                selectExpandDefinition: null
            );
            IMapper mapper = serviceProvider.GetRequiredService<IMapper>();

            //ACt
            var descriptor = mapper.Map<FormRequestDetailsDescriptor>(parameters);

            // Assert
            Assert.Equal("/api/Order/Get", descriptor.GetUrl);
            Assert.Equal("/api/Order/Add", descriptor.AddUrl);
            Assert.Equal("/api/Order/Update", descriptor.UpdateUrl);
            Assert.Equal("/api/Order/Delete", descriptor.DeleteUrl);
            Assert.Equal("MyApp.Domain.Entities.Order", descriptor.ModelType);
            Assert.Equal("MyApp.Data.Entities.Order", descriptor.DataType);
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
