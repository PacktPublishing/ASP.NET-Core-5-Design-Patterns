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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
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
        public AlarmTriggeredHandler(IMessageHandler next)
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
        public AlarmPausedHandler(IMessageHandler next)
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
        public AlarmStoppedHandler(IMessageHandler next)
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
}
