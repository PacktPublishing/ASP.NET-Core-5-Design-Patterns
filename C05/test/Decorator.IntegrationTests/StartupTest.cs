using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decorator.IntegrationTests
{
    public abstract class StartupTest<TStartup> : IClassFixture<WebApplicationFactory<TStartup>>
         where TStartup : class
    {
        private readonly WebApplicationFactory<TStartup> _webApplicationFactory;

        protected StartupTest(WebApplicationFactory<TStartup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));
        }

        [Fact]
        public async Task Should_return_a_double_decorated_string()
        {
            // Arrange
            var client = _webApplicationFactory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal(
                "Operation: <DecoratorB><DecoratorA>Hello from ComponentA</DecoratorA></DecoratorB>", 
                body
            );
        }
    }

    public class DecoratorPlainStartupTest : StartupTest<DecoratorPlain.Startup>
    {
        public DecoratorPlainStartupTest(WebApplicationFactory<DecoratorPlain.Startup> webApplicationFactory)
            : base(webApplicationFactory)
        {
        }
    }

    public class DecoratorScrutorStartupTest : StartupTest<DecoratorScrutor.Startup>
    {
        public DecoratorScrutorStartupTest(WebApplicationFactory<DecoratorScrutor.Startup> webApplicationFactory)
            : base(webApplicationFactory)
        {
        }
    }

}
