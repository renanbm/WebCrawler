using System.Collections.Generic;

namespace WebCrawler.Spider.Application.Interfaces
{
    public interface ISpiderAppService
    {
        ISet<string> Crawl(string baseUrl);
    }
}