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
}
