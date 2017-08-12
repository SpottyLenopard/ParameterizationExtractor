using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quipu.ParameterizationExtractor.Logic.Interfaces;
using Quipu.ParameterizationExtractor.Logic.MSSQL;
using Quipu.ParameterizationExtractor.Logic.Model;
using Microsoft.Extensions.Configuration;
using ParameterizationExtractor.WebUI.DataAccess;
using ParameterizationExtractor.Logic.MSSQL;
using ParameterizationExtractor.WebUI.Middlewares;

namespace ParameterizationExtractor.WebUI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("globalExtractConfig.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDependencyBuilder, DependencyBuilder>();
            services.AddTransient<ISqlBuilder, MSSqlBuilder>();
            services.AddScoped<ISourceSchema, MSSQLSourceSchema>();
            services.AddTransient<IObjectMetaDataProvider, ObjectMetaDataProvider>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddSingleton<ILog, Logger>();
            services.AddSingleton<IMetaDataInitializer, MetaDataInitializer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IExtractConfiguration, GlobalExtractConfiguration>(s => new GlobalExtractConfiguration());

            services.AddLogging();

            services.AddMvc();
            services.AddOptions();
            services.Configure<AppSettings>(_ => Configuration.GetSection("appsettings").Bind(_));
            services.Configure<GlobalExtractConfiguration>(_ => Configuration.GetSection("globalExtractConfig").Bind(_));
            services.AddSingleton<IExtractConfiguration, GlobalExtractConfiguration>(_ =>
            {
                var gb = new GlobalExtractConfiguration();
                Configuration.GetSection("globalExtractConfig").Bind(gb);
                return gb;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // order matters
            app.UseCustomErrorHandling();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {              
                app.UseDeveloperExceptionPage();
            }

            app.UseImpersonation();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}");
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
