using Microsoft.Extensions.Hosting;
using System;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using ForEvolve.Testing.Logging;
using Moq;
using System.Collections.Generic;

namespace Mediator.Tests
{
    public class MediatorTest
    {
        private readonly List<string> _lines = new List<string>();
        private readonly ILoggerFactory _loggerFactory;
        public MediatorTest(ITestOutputHelper output)
        {
            if (output == null) { throw new ArgumentNullException(nameof(output)); }
            //ILoggerProvider loggerProvider = new XunitTestOutputLoggerProvider(output);
            ILoggerProvider loggerProvider = new AssertableLoggerProvider(_lines);
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock
                .Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns((string categoryName) => loggerProvider.CreateLogger(categoryName));
            _loggerFactory = loggerFactoryMock.Object;
        }

        [Fact]
        public void Send_a_message_to_all_colleagues()
        {
            // Arrange
            var miller = new ConcreteColleague("Miller", _loggerFactory);
            var orazio = new ConcreteColleague("Orazio", _loggerFactory);
            var fletcher = new ConcreteColleague("Fletcher", _loggerFactory);

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
            Assert.Collection(_lines,
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
            var miller = new ConcreteColleague("Miller", _loggerFactory);
            var orazio = new ConcreteColleague("Orazio", _loggerFactory);
            var fletcher = new ConcreteColleague("Fletcher", _loggerFactory);

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
            // Assert
            Assert.Collection(_lines,
                line => Assert.Equal("[from: Miller][to: Miller]: Hey Orazio!", line),
                line => Assert.Equal("[from: Miller][to: Orazio]: Hey Orazio!", line),
                line => Assert.Equal("[from: Orazio][to: Miller]: What's up Miller?", line),
                line => Assert.Equal("[from: Orazio][to: Orazio]: What's up Miller?", line),
                line => Assert.Equal("[from: Miller][to: Miller]: Great Orazio, and you?", line),
                line => Assert.Equal("[from: Miller][to: Orazio]: Great Orazio, and you?", line)
            );
        }
    }
}
