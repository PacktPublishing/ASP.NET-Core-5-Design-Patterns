using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using Xunit;

namespace Mediator.Tests
{
    public class MediatorTest
    {
        [Fact]
        public void Send_a_message_to_all_colleagues()
        {
            // Arrange
            var (millerWriter, miller) = CreateConcreteColleague("Miller");
            var (orazioWriter, orazio) = CreateConcreteColleague("Orazio");
            var (fletcherWriter, fletcher) = CreateConcreteColleague("Fletcher");

            var mediator = new BroadcastMediator(miller, orazio, fletcher);
            var expectedOutput = @"[Miller]: Hey everyone!
[Orazio]: What's up Miller?
[Fletcher]: Hey Miller!
";

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
            Assert.Equal(expectedOutput, millerWriter.Output.ToString());
            Assert.Equal(expectedOutput, orazioWriter.Output.ToString());
            Assert.Equal(expectedOutput, fletcherWriter.Output.ToString());
        }

        [Fact]
        public void Send_a_direct_message_using_dependency_injection()
        {
            // Arrange
            var (millerWriter, miller) = CreateConcreteColleague("Miller");
            var (orazioWriter, orazio) = CreateConcreteColleague("Orazio");
            var (fletcherWriter, fletcher) = CreateConcreteColleague("Fletcher");

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
            Assert.Empty(fletcherWriter.Output.ToString());

            Assert.Equal(@"[Miller]: Hey Orazio!
[Orazio]: What's up Miller?
[Miller]: Great Orazio, and you?
", millerWriter.Output.ToString());

            Assert.Equal(@"[Miller]: Hey Orazio!
[Orazio]: What's up Miller?
[Miller]: Great Orazio, and you?
", orazioWriter.Output.ToString());
        }

        private (TestMessageWriter, ConcreteColleague) CreateConcreteColleague(string name)
        {
            var messageWriter = new TestMessageWriter();
            var concreateColleague = new ConcreteColleague(name, messageWriter);
            return (messageWriter, concreateColleague);
        }

        public class TestMessageWriter : IMessageWriter<Message>
        {
            public StringBuilder Output { get; } = new StringBuilder();

            public void Write(Message message)
            {
                Output.AppendLine($"[{message.Sender.Name}]: {message.Content}");
            }
        }
    }
}
