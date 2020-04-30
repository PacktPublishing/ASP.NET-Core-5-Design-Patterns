namespace ImprovedChainOfResponsibility
{
    public class AlarmPausedHandler : MessageHandlerBase
    {
        protected override string HandledMessageName => "AlarmPaused";

        public AlarmPausedHandler(IMessageHandler next = null) : base(next) { }

        protected override void Treat(Message message)
        {
            // Do something cleaver with the Payload
        }
    }
}
