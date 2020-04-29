#define USE_CLEAN_SERVICE

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ClearName
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var strings = "Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua"
                .Split(' ');
#if USE_CLEAN_SERVICE
            services.AddSingleton<IExampleService>(x => new CleanExampleService(strings));
#else
            services.AddSingleton<IExampleService>(x => new OneMethodExampleService(strings));
#endif
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IExampleService exampleService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var result = exampleService.RandomizeOneString();
                var json = JsonConvert.SerializeObject(result, Formatting.Indented);

                context.Response.Headers.Add("Content-Type", "application/json");
                await context.Response.WriteAsync($"Result: {json}");
            });
        }
    }
}
