namespace FinalChainOfResponsibility
{
    public class SomeMultiHandler : MultipleMessageHandlerBase
    {
        public SomeMultiHandler(IMessageHandler next = null)
            : base(next) { }

        protected override string[] HandledMessagesName
            => new string[] { "Foo", "Bar", "Baz" };

        protected override void Treat(Message message)
        {
            // Do something cleaver with the Payload
        }
    }
}
