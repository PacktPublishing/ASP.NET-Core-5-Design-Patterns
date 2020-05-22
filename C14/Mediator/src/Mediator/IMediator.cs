using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Mediator
{
    public interface IMediator
    {
        void Send(Message message);
    }

    public interface IColleague
    {
        string Name { get; }
        void ReceiveMessage(Message message);
    }

    public class Message
    {
        public Message(IColleague from, string content)
        {
            Sender = from ?? throw new ArgumentNullException(nameof(from));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public IColleague Sender { get; }
        public string Content { get; }
    }

    public class BroadcastMediator : IMediator
    {
        private readonly List<IColleague> _colleagues;
        public BroadcastMediator(params IColleague[] colleagues)
        {
            if (colleagues == null) { throw new ArgumentNullException(nameof(colleagues)); }
            _colleagues = new List<IColleague>(colleagues);
        }

        public void Send(Message message)
        {
            foreach (var colleague in _colleagues)
            {
                colleague.ReceiveMessage(message);
            }
        }
    }

    public class DirectMessageMediator : IMediator
    {
        private readonly List<IColleague> _colleagues;
        public DirectMessageMediator(params IColleague[] colleagues)
        {
            if (colleagues == null) { throw new ArgumentNullException(nameof(colleagues)); }
            _colleagues = new List<IColleague>(colleagues);
        }

        public void Send(Message message)
        {
            foreach (var colleague in _colleagues)
            {
                if (message.Sender == colleague || message.Content.Contains(colleague.Name))
                {
                    colleague.ReceiveMessage(message);
                }
            }
        }
    }

    public class ConcreteColleague : IColleague
    {
        private readonly ILogger _logger;
        public ConcreteColleague(string name, ILoggerFactory logger)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _logger = logger.CreateLogger(Name);
        }

        public string Name { get; }

        public void ReceiveMessage(Message message)
        {
            _logger.LogInformation($"[from: {message.Sender.Name}][to: {Name}]: {message.Content}");
        }
    }
}
