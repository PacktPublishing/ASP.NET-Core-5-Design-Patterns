namespace FinalChainOfResponsibility
{
    public class AlarmStoppedHandler : SingleMessageHandlerBase
    {
        protected override string HandledMessageName => "AlarmStopped";

        public AlarmStoppedHandler(IMessageHandler next = null)
            : base(next) { }

        protected override void Treat(Message message)
        {
            // Do something cleaver with the Payload
        }
    }
}
