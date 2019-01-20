namespace ImprovedChainOfResponsibility
{
    public class AlarmTriggeredHandler : MessageHandlerBase
    {
        protected override string HandledMessageName => "AlarmTriggered";

        public AlarmTriggeredHandler(IMessageHandler next = null) : base(next) { }

        protected override void Treat(Message message)
        {
            // Do something cleaver with the Payload
        }
    }
}
