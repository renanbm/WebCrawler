using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebCrawler.CrossCutting;
using WebCrawler.Domain.Interfaces;
using WebCrawler.Repository;
using WebCrawler.Spider.Application;
using WebCrawler.Spider.Application.Interfaces;

namespace WebCrawler.Spider.Tests
{
    [TestClass]
    public class SpiderTests
    {
        private IConfigurationRoot Configuration { get; set; }

        private readonly ISpiderAppService _spiderAppService;

        public SpiderTests()
        {
            var credentials = GetCredentials();
            var repository = new SpiderRepository(credentials);
            _spiderAppService = new SpiderAppService(repository);
        }

        [TestMethod]
        public void Crawler_Obtem_121_Urls_No_4Devs()
        {
            Assert.AreEqual(121, _spiderAppService.Crawl("http://www.4devs.com.br/").Count);
        }

        private Credentials GetCredentials()
        {
            var credentialsFilePath = Path.GetFullPath("../../../../../src/WebCrawler.Spider.Web");
            var configBuilder = new ConfigurationBuilder().AddJsonFile(Path.Combine(credentialsFilePath, "vcap-local.json"), true);
            Configuration = configBuilder.Build();

            return new Credentials
            {
                host = Configuration["host"],
                password = Configuration["password"],
                username = Configuration["username"]
            };
        }
    }
}
