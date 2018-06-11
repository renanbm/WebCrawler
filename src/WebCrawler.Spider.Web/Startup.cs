using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebCrawler.CrossCutting.IoC;

namespace WebCrawler.Spider.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("vcap-local.json", true);
            Configuration = configBuilder.Build();

            Configuration["cloudantNoSQLDB:0:credentials:username"] = Configuration["username"];
            Configuration["cloudantNoSQLDB:0:credentials:password"] = Configuration["password"];
            Configuration["cloudantNoSQLDB:0:credentials:host"] = Configuration["host"];

            var vcapServices = Environment.GetEnvironmentVariable("VCAP_SERVICES");
            if (vcapServices == null) return;

            dynamic json = JsonConvert.DeserializeObject(vcapServices);
            foreach (var obj in json.Children())
            {
                if (!((string)obj.Name).ToLowerInvariant().Contains("cloudant")) continue;
                var credentials = (((JProperty)obj).Value[0] as dynamic).credentials;
                if (credentials == null) continue;
                string host = credentials.host;
                string username = credentials.username;
                string password = credentials.password;
                Configuration["cloudantNoSQLDB:0:credentials:username"] = username;
                Configuration["cloudantNoSQLDB:0:credentials:password"] = password;
                Configuration["cloudantNoSQLDB:0:credentials:host"] = host;
                break;
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services, Configuration);
        }
    }
}
