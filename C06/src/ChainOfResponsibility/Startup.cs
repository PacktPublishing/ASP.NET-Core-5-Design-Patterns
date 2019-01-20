using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChainOfResponsibility
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageHandler>(new AlarmTriggeredHandler(new AlarmPausedHandler(new AlarmStoppedHandler(new DefaultHandler()))));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMessageHandler messageHandler)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var message = new Message
                {
                    Name = context.Request.Query["name"],
                    Payload = context.Request.Query["payload"]
                };
                try
                {
                    messageHandler.Handle(message);
                    await context.Response.WriteAsync($"Message '{message.Name}' handled successfully.");
                }
                catch (NotSupportedException ex)
                {
                    await context.Response.WriteAsync(ex.Message);
                }
            });
        }
    }

    public interface IMessageHandler
    {
        void Handle(Message message);
    }

    public class Message
    {
        public string Name { get; set; }
        public string Payload { get; set; }
    }

    public class AlarmTriggeredHandler : IMessageHandler
    {
        private readonly IMessageHandler _next;
        public AlarmTriggeredHandler(IMessageHandler next = null)
        {
            _next = next;
        }

        public void Handle(Message message)
        {
            if (message.Name == "AlarmTriggered")
            {
                // Do something cleaver with the Payload
            }
            else if (_next != null)
            {
                _next.Handle(message);
            }
        }
    }

    public class AlarmPausedHandler : IMessageHandler
    {
        private readonly IMessageHandler _next;
        public AlarmPausedHandler(IMessageHandler next = null)
        {
            _next = next;
        }

        public void Handle(Message message)
        {
            if (message.Name == "AlarmPaused")
            {
                // Do something cleaver with the Payload
            }
            else if (_next != null)
            {
                _next.Handle(message);
            }
        }
    }

    public class AlarmStoppedHandler : IMessageHandler
    {
        private readonly IMessageHandler _next;
        public AlarmStoppedHandler(IMessageHandler next = null)
        {
            _next = next;
        }

        public void Handle(Message message)
        {
            if (message.Name == "AlarmStopped")
            {
                // Do something cleaver with the Payload
            }
            else if (_next != null)
            {
                _next.Handle(message);
            }
        }
    }

    public class DefaultHandler : IMessageHandler
    {
        public void Handle(Message message)
        {
            throw new NotSupportedException($"Message named '{message.Name}' are not supported.");
        }
    }
}
