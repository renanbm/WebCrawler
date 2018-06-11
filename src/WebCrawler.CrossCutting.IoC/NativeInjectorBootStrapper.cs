using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Domain.Interfaces;
using WebCrawler.Repository;
using WebCrawler.Spider.Application;
using WebCrawler.Spider.Application.Interfaces;

namespace WebCrawler.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var creds = new Credentials
            {
                username = configuration["cloudantNoSQLDB:0:credentials:username"],
                password = configuration["cloudantNoSQLDB:0:credentials:password"],
                host = configuration["cloudantNoSQLDB:0:credentials:host"]
            };

            services.AddSingleton(typeof(Credentials), creds);

            services.AddTransient<ISpiderAppService, SpiderAppService>();
            services.AddTransient<ISpiderRepository, SpiderRepository>();
        }
    }
}
