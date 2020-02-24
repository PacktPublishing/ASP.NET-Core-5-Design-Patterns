using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommonScenarios
{
    public class DefaultOptionsTests
    {
        public const string DefaultOptionName = "Default Options";
        private IServiceProvider _serviceProvider;

        public DefaultOptionsTests()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<MyOptions>(myOptions =>
            {
                myOptions.Name = DefaultOptionName;
            });
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void OptionsSnapshot()
        {
            var snapshot = _serviceProvider.GetRequiredService<IOptionsSnapshot<MyOptions>>();
            Assert.NotNull(snapshot.Value);
            Assert.Equal(DefaultOptionName, snapshot.Value.Name);
        }

        [Fact]
        public void OptionsMonitor()
        {
            var monitor = _serviceProvider.GetRequiredService<IOptionsMonitor<MyOptions>>();
            Assert.NotNull(monitor.CurrentValue);
            Assert.Equal(DefaultOptionName, monitor.CurrentValue.Name);
        }
    }
}
