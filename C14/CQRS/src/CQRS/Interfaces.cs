using System;
using System.Collections.Generic;

namespace CQRS
{
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
}
