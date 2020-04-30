using System;

namespace FinalChainOfResponsibility
{
    public class DefaultHandler : IMessageHandler
    {
        public void Handle(Message message)
        {
            throw new NotSupportedException($"Message named '{message.Name}' are not supported.");
        }
    }
}
