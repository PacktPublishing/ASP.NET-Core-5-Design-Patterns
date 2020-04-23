using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace OptionsValidation
{
    public class ConfigureMeWithDataAnnotations
    {
        [Fact]
        public void Should_pass_validation()
        {
            var services = new ServiceCollection();
            services.AddOptions<Options>()
                .Configure(o => o.MyImportantProperty = "Some important value")
                .ValidateDataAnnotations();
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptionsMonitor<Options>>();
            Assert.Equal("Some important value", options.CurrentValue.MyImportantProperty);
        }

        [Fact]
        public void Should_fail_validation()
        {
            var services = new ServiceCollection();
            services.AddOptions<Options>()
                .ValidateDataAnnotations();
            var serviceProvider = services.BuildServiceProvider();
            var error = Assert.Throws<OptionsValidationException>(()
                => serviceProvider.GetService<IOptionsMonitor<Options>>().CurrentValue);
            Assert.Collection(error.Failures,
                f => Assert.Equal("DataAnnotation validation failed for members: 'MyImportantProperty' with the error: 'The MyImportantProperty field is required.'.", f)
            );
        }

        private class Options
        {
            [Required]
            public string MyImportantProperty { get; set; }
        }
    }
}
