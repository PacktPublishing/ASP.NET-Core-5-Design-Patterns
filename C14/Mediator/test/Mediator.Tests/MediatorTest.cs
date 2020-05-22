using Microsoft.Extensions.Hosting;
using System;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using ForEvolve.Testing.Logging;
using Moq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Mediator.Tests
{
    public class MediatorTest
    {
        [Fact]
        public void Send_a_message_to_all_colleagues()
        {
            // Arrange
            var lines = new List<string>();
            var loggerFactory = CreateLoggerFactory(lines);

            var miller = new ConcreteColleague("Miller", loggerFactory);
            var orazio = new ConcreteColleague("Orazio", loggerFactory);
            var fletcher = new ConcreteColleague("Fletcher", loggerFactory);

            var mediator = new BroadcastMediator(miller, orazio, fletcher);

            // Act
            mediator.Send(new Message(
                from: miller,
                content: "Hey everyone!"
            ));
            mediator.Send(new Message(
                from: orazio,
                content: "What's up Miller?"
            ));
            mediator.Send(new Message(
                from: fletcher,
                content: "Hey Miller!"
            ));

            // Assert
            Assert.Collection(lines,
                line => Assert.Equal("[from: Miller][to: Miller]: Hey everyone!", line),
                line => Assert.Equal("[from: Miller][to: Orazio]: Hey everyone!", line),
                line => Assert.Equal("[from: Miller][to: Fletcher]: Hey everyone!", line),

                line => Assert.Equal("[from: Orazio][to: Miller]: What's up Miller?", line),
                line => Assert.Equal("[from: Orazio][to: Orazio]: What's up Miller?", line),
                line => Assert.Equal("[from: Orazio][to: Fletcher]: What's up Miller?", line),

                line => Assert.Equal("[from: Fletcher][to: Miller]: Hey Miller!", line),
                line => Assert.Equal("[from: Fletcher][to: Orazio]: Hey Miller!", line),
                line => Assert.Equal("[from: Fletcher][to: Fletcher]: Hey Miller!", line)
            );
        }

        [Fact]
        public void Send_a_direct_message()
        {
            // Arrange
            var lines = new List<string>();
            var loggerFactory = CreateLoggerFactory(lines);

            var miller = new ConcreteColleague("Miller", loggerFactory);
            var orazio = new ConcreteColleague("Orazio", loggerFactory);
            var fletcher = new ConcreteColleague("Fletcher", loggerFactory);

            var mediator = new DirectMessageMediator(miller, orazio, fletcher);

            // Act
            mediator.Send(new Message(
                from: miller,
                content: "Hey Orazio!"
            ));
            mediator.Send(new Message(
                from: orazio,
                content: "What's up Miller?"
            ));
            mediator.Send(new Message(
                from: miller,
                content: "Great Orazio, and you?"
            ));

            // Assert
            Assert.Collection(lines,
                line => Assert.Equal("[from: Miller][to: Miller]: Hey Orazio!", line),
                line => Assert.Equal("[from: Miller][to: Orazio]: Hey Orazio!", line),
                line => Assert.Equal("[from: Orazio][to: Miller]: What's up Miller?", line),
                line => Assert.Equal("[from: Orazio][to: Orazio]: What's up Miller?", line),
                line => Assert.Equal("[from: Miller][to: Miller]: Great Orazio, and you?", line),
                line => Assert.Equal("[from: Miller][to: Orazio]: Great Orazio, and you?", line)
            );
        }

        [Fact]
        public void Send_a_direct_message_using_dependency_injection()
        {
            // Arrange
            var lines = new List<string>();
            var loggerFactory = CreateLoggerFactory(lines);

            var miller = new ConcreteColleague("Miller", loggerFactory);
            var orazio = new ConcreteColleague("Orazio", loggerFactory);
            var fletcher = new ConcreteColleague("Fletcher", loggerFactory);

            var services = new ServiceCollection();
            services.AddSingleton<IMediator, DirectMessageMediator>();
            services.AddSingleton(serviceProvider => serviceProvider.GetServices<IColleague>().ToArray());
            services.AddSingleton<IColleague>(miller);
            services.AddSingleton<IColleague>(orazio);
            services.AddSingleton<IColleague>(fletcher);

            var serviceProvider = services.BuildServiceProvider();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            // Act
            mediator.Send(new Message(
                from: miller,
                content: "Hey Orazio!"
            ));
            mediator.Send(new Message(
                from: orazio,
                content: "What's up Miller?"
            ));
            mediator.Send(new Message(
                from: miller,
                content: "Great Orazio, and you?"
            ));

            // Assert
            Assert.Collection(lines,
                line => Assert.Equal("[from: Miller][to: Miller]: Hey Orazio!", line),
                line => Assert.Equal("[from: Miller][to: Orazio]: Hey Orazio!", line),
                line => Assert.Equal("[from: Orazio][to: Miller]: What's up Miller?", line),
                line => Assert.Equal("[from: Orazio][to: Orazio]: What's up Miller?", line),
                line => Assert.Equal("[from: Miller][to: Miller]: Great Orazio, and you?", line),
                line => Assert.Equal("[from: Miller][to: Orazio]: Great Orazio, and you?", line)
            );
        }

        private ILoggerFactory CreateLoggerFactory(IList<string> lines)
        {
            ILoggerProvider loggerProvider = new AssertableLoggerProvider(lines);
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock
                .Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns((string categoryName) => loggerProvider.CreateLogger(categoryName));
            return loggerFactoryMock.Object;
        }
    }
}
