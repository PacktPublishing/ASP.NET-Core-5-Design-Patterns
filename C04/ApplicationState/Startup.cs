using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
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

public class SomeService
{
    private readonly IMyApplicationWideService _myApplicationWideService;

    public SomeService(IMyApplicationWideService myApplicationWideService)
    {
        _myApplicationWideService = myApplicationWideService ?? throw new ArgumentNullException(nameof(myApplicationWideService));
    }

    public string DoSomething()
    {
        if (_myApplicationWideService.Has<string>("some-key"))
        {
            return _myApplicationWideService.Get<string>("some-key");
        }
        return null;
    }
}

public interface IMyApplicationWideService
{
    TItem Get<TItem>(string key);
    bool Has<TItem>(string key);
}

public class MyApplicationWideServiceImplementation : IMyApplicationWideService
{
    private readonly IMemoryCache _memoryCache;

    public MyApplicationWideServiceImplementation(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public TItem Get<TItem>(string key)
    {
        return _memoryCache.Get<TItem>(key);
    }

    public bool Has<TItem>(string key)
    {
        return _memoryCache.TryGetValue<TItem>(key, out _);
    }
}
}
