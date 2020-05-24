using System;
using System.Collections.Generic;

namespace CQRS
{
    #region Interfaces

    public interface IParticipant
    {
        string Name { get; }
        void Join(IChatRoom chatRoom);
        void Leave(IChatRoom chatRoom);
        void SendMessageTo(IChatRoom chatRoom, string message);
        void NewMessageReceivedFrom(IChatRoom chatRoom, ChatMessage message);
        IEnumerable<IParticipant> ListParticipantsOf(IChatRoom chatRoom);
        IEnumerable<ChatMessage> ListMessagesOf(IChatRoom chatRoom);
    }

    public interface IChatRoom
    {
        string Name { get; }

        void Add(IParticipant participant);
        void Remove(IParticipant participant);
        IEnumerable<IParticipant> ListParticipants();

        void Add(ChatMessage message);
        IEnumerable<ChatMessage> ListMessages();
    }

    public interface IMediator
    {
        TReturn Send<TCommand, TReturn>(TCommand query)
            where TCommand: ICommand<TReturn>;
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;

        void Register<TCommand>(IVoidHandler<TCommand> commandHandler)
            where TCommand : ICommand;
        void Register<TCommand, TReturn>(IReturnHandler<TCommand, TReturn> commandHandler)
            where TCommand : ICommand<TReturn>;
    }

    public interface ICommand { }
    public interface IVoidHandler<TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }

    public interface ICommand<TReturn> { }
    public interface IReturnHandler<TCommand, TReturn>
        where TCommand : ICommand<TReturn>
    {
        TReturn Handle(TCommand query);
    }

    public class ChatMessage
    {
        public ChatMessage(IParticipant sender, string message)
        {
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Date = DateTime.UtcNow;
        }

        public DateTime Date { get; }
        public IParticipant Sender { get; }
        public string Message { get; }
    }

    #endregion

    #region Commands

    public class JoinChatRoom
    {
        public class Command : ICommand
        {
            public Command(IChatRoom chatRoom, IParticipant requester)
            {
                ChatRoom = chatRoom ?? throw new ArgumentNullException(nameof(chatRoom));
                Requester = requester ?? throw new ArgumentNullException(nameof(requester));
            }

            public IChatRoom ChatRoom { get; }
            public IParticipant Requester { get; }
        }

        public class Handler : IVoidHandler<Command>
        {
            public void Handle(Command command)
            {
                command.ChatRoom.Add(command.Requester);
            }
        }
    }

    public class LeaveChatRoom
    {
        public class Command : ICommand
        {
            public Command(IChatRoom chatRoom, IParticipant requester)
            {
                ChatRoom = chatRoom ?? throw new ArgumentNullException(nameof(chatRoom));
                Requester = requester ?? throw new ArgumentNullException(nameof(requester));
            }

            public IChatRoom ChatRoom { get; }
            public IParticipant Requester { get; }
        }

        public class Handler : IVoidHandler<Command>
        {
            public void Handle(Command command)
            {
                command.ChatRoom.Remove(command.Requester);
            }
        }
    }

    public class SendChatMessage
    {
        public class Command : ICommand
        {
            public Command(IChatRoom chatRoom, ChatMessage message)
            {
                ChatRoom = chatRoom ?? throw new ArgumentNullException(nameof(chatRoom));
                Message = message ?? throw new ArgumentNullException(nameof(message));
            }

            public IChatRoom ChatRoom { get; }
            public ChatMessage Message { get; }
        }

        public class Handler : IVoidHandler<Command>
        {
            public void Handle(Command command)
            {
                command.ChatRoom.Add(command.Message);
                foreach (var participant in command.ChatRoom.ListParticipants())
                {
                    participant.NewMessageReceivedFrom(command.ChatRoom, command.Message);
                }
            }
        }
    }

    public class ListParticipants
    {
        public class Command : ICommand<IEnumerable<IParticipant>>
        {
            public Command(IChatRoom chatRoom, IParticipant requester)
            {
                Requester = requester ?? throw new ArgumentNullException(nameof(requester));
                ChatRoom = chatRoom ?? throw new ArgumentNullException(nameof(chatRoom));
            }

            public IParticipant Requester { get; }
            public IChatRoom ChatRoom { get; }
        }

        public class Handler : IReturnHandler<Command, IEnumerable<IParticipant>>
        {
            public IEnumerable<IParticipant> Handle(Command query)
            {
                return query.ChatRoom.ListParticipants();
            }
        }
    }

    public class ListMessages
    {
        public class Command : ICommand<IEnumerable<ChatMessage>>
        {
            public Command(IChatRoom chatRoom, IParticipant requester)
            {
                Requester = requester ?? throw new ArgumentNullException(nameof(requester));
                ChatRoom = chatRoom ?? throw new ArgumentNullException(nameof(chatRoom));
            }

            public IParticipant Requester { get; }
            public IChatRoom ChatRoom { get; }
        }

        public class Handler : IReturnHandler<Command, IEnumerable<ChatMessage>>
        {
            public IEnumerable<ChatMessage> Handle(Command query)
            {
                return query.ChatRoom.ListMessages();
            }
        }
    }

    #endregion

    #region Implementations

    public class Participant : IParticipant
    {
        private readonly IMediator _mediator;
        private readonly IMessageWriter _messageWriter;
        public Participant(IMediator mediator, string name, IMessageWriter messageWriter)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _messageWriter = messageWriter ?? throw new ArgumentNullException(nameof(messageWriter));
        }

        public string Name { get; }

        public void Join(IChatRoom chatRoom)
        {
            _mediator.Send(new JoinChatRoom.Command(chatRoom, this));
        }

        public void Leave(IChatRoom chatRoom)
        {
            _mediator.Send(new LeaveChatRoom.Command(chatRoom, this));
        }

        public IEnumerable<ChatMessage> ListMessagesOf(IChatRoom chatRoom)
        {
            return _mediator.Send<ListMessages.Command, IEnumerable<ChatMessage>>(new ListMessages.Command(chatRoom, this));
        }

        public IEnumerable<IParticipant> ListParticipantsOf(IChatRoom chatRoom)
        {
            return _mediator.Send<ListParticipants.Command, IEnumerable<IParticipant>>(new ListParticipants.Command(chatRoom, this));
        }

        public void NewMessageReceivedFrom(IChatRoom chatRoom, ChatMessage message)
        {
            _messageWriter.Write(chatRoom, message);
        }

        public void SendMessageTo(IChatRoom chatRoom, string message)
        {
            _mediator.Send(new SendChatMessage.Command(chatRoom, new ChatMessage(this, message)));
        }
    }

    public class ChatRoom : IChatRoom
    {
        private readonly List<IParticipant> _participants = new List<IParticipant>();
        private readonly List<ChatMessage> _chatMessages = new List<ChatMessage>();

        public ChatRoom(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public void Add(IParticipant participant)
        {
            _participants.Add(participant);
        }

        public void Add(ChatMessage message)
        {
            _chatMessages.Add(message);
        }

        public IEnumerable<ChatMessage> ListMessages()
        {
            return _chatMessages.AsReadOnly();
        }

        public IEnumerable<IParticipant> ListParticipants()
        {
            return _participants.AsReadOnly();
        }

        public void Remove(IParticipant participant)
        {
            _participants.Remove(participant);
        }
    }

    public class Mediator : IMediator
    {
        private readonly HandlerDictionary _handlers = new HandlerDictionary();

        public void Register<TCommand>(IVoidHandler<TCommand> commandHandler)
            where TCommand : ICommand
        {
            _handlers.AddHandler(commandHandler);
        }

        public void Register<TCommand, TReturn>(IReturnHandler<TCommand, TReturn> commandHandler)
            where TCommand : ICommand<TReturn>
        {
            _handlers.AddHandler(commandHandler);
        }

        public TReturn Send<TCommand, TReturn>(TCommand query)
            where TCommand : ICommand<TReturn>
        {
            var handler = _handlers.Find<TCommand, TReturn>();
            return handler.Handle(query);
        }

        public void Send<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var handlers = _handlers.FindAll<TCommand>();
            foreach (var handler in handlers)
            {
                handler.Handle(command);
            }
        }

        private class HandlerList
        {
            private readonly List<object> _voidHandlers = new List<object>();
            private readonly List<object> _returnHandlers = new List<object>();

            public void Add<TCommand>(IVoidHandler<TCommand> handler)
                where TCommand : ICommand
            {
                _voidHandlers.Add(handler);
            }

            public void Add<TCommand, TReturn>(IReturnHandler<TCommand, TReturn> handler)
                where TCommand : ICommand<TReturn>
            {
                _returnHandlers.Add(handler);
            }

            public IEnumerable<IVoidHandler<TCommand>> FindAll<TCommand>()
                where TCommand : ICommand
            {
                foreach (var handler in _voidHandlers)
                {
                    if (handler is IVoidHandler<TCommand> output)
                    {
                        yield return output;
                    }
                }
            }
            public IReturnHandler<TCommand, TReturn> Find<TCommand, TReturn>()
                where TCommand : ICommand<TReturn>
            {
                foreach (var handler in _returnHandlers)
                {
                    if (handler is IReturnHandler<TCommand, TReturn> output)
                    {
                        return output;
                    }
                }
                throw new ReturnHandlerNotFoundException(typeof(TCommand));
            }
        }

        public class HandlerDictionary
        {
            private readonly Dictionary<Type, HandlerList> _handlers = new Dictionary<Type, HandlerList>();

            public void AddHandler<TCommand>(IVoidHandler<TCommand> handler)
                where TCommand : ICommand
            {
                var type = typeof(TCommand);
                EnforceTypeEntry(type);
                var registeredHandlers = _handlers[type];
                registeredHandlers.Add(handler);
            }


            public void AddHandler<TCommand, TReturn>(IReturnHandler<TCommand, TReturn> handler)
                where TCommand : ICommand<TReturn>
            {
                var type = typeof(TCommand);
                EnforceTypeEntry(type);
                var registeredHandlers = _handlers[type];
                registeredHandlers.Add(handler);
            }

            public IEnumerable<IVoidHandler<TCommand>> FindAll<TCommand>()
                where TCommand : ICommand
            {
                var type = typeof(TCommand);
                EnforceTypeEntry(type);
                var registeredHandlers = _handlers[type];
                return registeredHandlers.FindAll<TCommand>();
            }

            public IReturnHandler<TCommand, TReturn> Find<TCommand, TReturn>()
                where TCommand : ICommand<TReturn>
            {
                var type = typeof(TCommand);
                EnforceTypeEntry(type);
                var registeredHandlers = _handlers[type];
                return registeredHandlers.Find<TCommand, TReturn>();
            }

            private void EnforceTypeEntry(Type type)
            {
                if (!_handlers.ContainsKey(type))
                {
                    _handlers.Add(type, new HandlerList());
                }
            }
        }
    }

    public class ReturnHandlerNotFoundException : Exception
    {
        public ReturnHandlerNotFoundException(Type queryType)
            : base($"No return handler found for query '{queryType}'.")
        {
        }
    }
    #endregion
}
