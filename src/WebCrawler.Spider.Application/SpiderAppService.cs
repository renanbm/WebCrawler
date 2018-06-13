using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.CrossCutting;
using WebCrawler.Domain.Interfaces;
using WebCrawler.Spider.Application.Interfaces;

namespace WebCrawler.Spider.Application
{
    public class SpiderAppService : ISpiderAppService
    {
        private readonly ISpiderRepository _spiderRepository;

        public SpiderAppService(ISpiderRepository spiderRepository)
        {
            _spiderRepository = spiderRepository;
        }

        public ISet<string> Crawl(string baseUrl)
        {
            baseUrl.FormatUrl();

            if (string.IsNullOrEmpty(baseUrl) || !baseUrl.HasValidDomainName())
                throw new Exception("The Url is invalid.");

            var spider = new Domain.Entities.Spider(baseUrl, _spiderRepository);

            while (spider.SpiderData.Queue.Count != 0)
                Parallel.ForEach(spider.SpiderData.Queue, itemQueue => { Task.Run(spider.CrawlPage).Wait(); });

            return spider.SpiderData.Crawled;
        }
    }
}