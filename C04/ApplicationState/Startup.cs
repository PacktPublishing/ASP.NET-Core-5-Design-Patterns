using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationState
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IMyApplicationWideService, MyApplicationWideServiceImplementation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMyApplicationWideService myAppState)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                if (context.Request.Method == "GET")
                {
                    await HandleGetRequestAsync(myAppState, context);
                }
                else
                {
                    await HandlePostRequestAsync(myAppState, context);
                }
            });
        }

        private static async Task HandleGetRequestAsync(IMyApplicationWideService myAppState, HttpContext context)
        {
            var key = context.Request.Query["key"];
            if (key.Count != 1)
            {
                await context.Response.WriteAsync("You must specify a single 'key' parameter like '?key=SomeAppStateKey'.");
                return;
            }
            var value = myAppState.Get<string>(key.Single());
            await context.Response.WriteAsync($"{key} = {value ?? "null"}");
        }

        private async Task HandlePostRequestAsync(IMyApplicationWideService myAppState, HttpContext context)
        {
            var key = context.Request.Form["key"].SingleOrDefault();
            var value = context.Request.Form["value"].SingleOrDefault();
            if (key == null || value == null)
            {
                await context.Response.WriteAsync("You must specify both a 'key' and a 'value'.");
                return;
            }
            myAppState.Set(key, value);
            await context.Response.WriteAsync($"{key} = {value ?? "null"}");
        }
    }
}
