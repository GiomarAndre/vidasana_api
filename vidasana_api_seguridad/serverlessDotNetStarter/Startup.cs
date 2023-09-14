using AwsDotnetCsharp.Providers.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AwsDotnetCsharp.Providers.Service;

namespace AwsDotnetCsharp
{
    public static class Startup
    {
        //private static readonly IConfiguration _configuration;
        public static ServiceProvider Services {get; private set;}

        public static void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ISeguridadRepository, SeguridadRepository>();
            serviceCollection.AddSingleton<ISeguridadService, SeguridadService>();

            serviceCollection.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.AllowAnyOrigin()
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                        });
                });

            Services = serviceCollection.BuildServiceProvider();
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();
            app.UseCors();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        /// </summary>
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.UseCors();
            /*app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });*/
        }        
    }
}