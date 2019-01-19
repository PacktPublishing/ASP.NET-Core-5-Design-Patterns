using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateMethod
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SearchMachine>(x => new LinearSearchMachine(1, 10, 5, 2, 123, 333, 4));
            services.AddSingleton<SearchMachine>(x => new BinarySearchMachine(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<SearchMachine> searchMachines)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                var elementsToFind = new int[] { 1, 10, 11 };
                await context.WriteLineAsync("<pre>");
                foreach (var searchMachine in searchMachines)
                {
                    var heading = $"Current search machine is {searchMachine.GetType().Name}";
                    await context.WriteLineAsync("".PadRight(heading.Length, '='));
                    await context.WriteLineAsync(heading);
                    foreach (var value in elementsToFind)
                    {
                        var index = searchMachine.IndexOf(value);
                        var wasFound = index.HasValue;
                        if (wasFound)
                        {
                            await context.WriteLineAsync($"The element '{value}' was found at index {index.Value}.");
                        }
                        else
                        {
                            await context.WriteLineAsync($"The element '{value}' was not found.");
                        }
                    }
                }
                await context.WriteLineAsync("</pre>");
            });
        }
    }

    internal static class HttpContextExtensions
    {
        public static async Task WriteLineAsync(this HttpContext context, string text)
        {
            await context.Response.WriteAsync(text);
            await context.Response.WriteAsync(Environment.NewLine);
        }
    }

    public abstract class SearchMachine
    {
        protected int[] Values { get; }

        protected SearchMachine(params int[] values)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public int? IndexOf(int value)
        {
            var result = Find(value);
            if (result < 0) { return null; }
            return result;
        }
        public abstract int Find(int value);
    }

    public class LinearSearchMachine : SearchMachine
    {
        public LinearSearchMachine(params int[] values) : base(values) { }

        public override int Find(int value)
        {
            var index = -1;
            foreach (var item in Values)
            {
                index++;
                if (item == value)
                {
                    break;
                }
            }
            return index;
        }
    }

    public class BinarySearchMachine : SearchMachine
    {
        public BinarySearchMachine(params int[] values) : base(values) { }

        public override int Find(int value)
        {
            return Array.BinarySearch(Values, value);
        }
    }
}
