namespace ImprovedChainOfResponsibility
{
    public class AlarmStoppedHandler : MessageHandlerBase
    {
        protected override string HandledMessageName => "AlarmStopped";

        public AlarmStoppedHandler(IMessageHandler next = null) : base(next) { }

        protected override void Treat(Message message)
        {
            // Do something cleaver with the Payload
        }
    }
}
