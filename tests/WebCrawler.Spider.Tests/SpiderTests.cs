using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebCrawler.CrossCutting;
using WebCrawler.Domain.Interfaces;
using WebCrawler.Repository;
using WebCrawler.Spider.Application;

namespace WebCrawler.Spider.Tests
{
    [TestClass]
    public class SpiderTests
    {
        private readonly ISpiderRepository _spiderRepository;

        public SpiderTests()
        {
            _spiderRepository = new SpiderRepository(new Credentials
            {
                host = "",
                password = "",
                username = ""
            });
        }

        [TestMethod]
        public void RodaTudo()
        {
            var spiderAppService = new SpiderAppService(_spiderRepository);

            Assert.AreEqual(121, spiderAppService.Crawl("http://www.4devs.com.br/").Count);
        }
    }
}
