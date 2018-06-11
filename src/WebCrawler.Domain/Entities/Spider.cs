using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebCrawler.CrossCutting;
using WebCrawler.Domain.Interfaces;
using WebCrawler.Domain.Results;

namespace WebCrawler.Domain.Entities
{
    public class Spider
    {
        public SpiderData SpiderData { get; }

        private string DomainName { get; }

        private readonly ISpiderRepository _spiderRepository;

        public Spider(string baseUrl, ISpiderRepository spiderRepository)
        {
            _spiderRepository = spiderRepository;
            SpiderData = new SpiderData(baseUrl);
            DomainName = baseUrl.GetDomainName();
            CreateDocument();
        }

        public async Task CrawlPage()
        {
            SpiderData.Queue.TryDequeue(out var pageUrl);

            pageUrl = UrlHelper.FormatUrl(SpiderData.BaseUrl, pageUrl);

            if (string.IsNullOrEmpty(pageUrl)) return;

            if (!SpiderData.Crawled.Contains(pageUrl))
            {
                var gatheredLinks = GatherLinks(pageUrl);

                lock (SpiderData)
                {
                    AddLinksToQueue(gatheredLinks);
                    SpiderData.Crawled.Add(pageUrl);
                }
            }

            await UpdateDocuments();
        }

        private IEnumerable<string> GatherLinks(string pageUrl)
        {
            try
            {
                var document = new HtmlWeb().Load(pageUrl);

                return document.DocumentNode.Descendants("a")
                    .Select(anchor => UrlHelper.FormatUrl(SpiderData.BaseUrl, anchor.GetAttributeValue("href", null)))
                    .Where(link => !string.IsNullOrEmpty(link));
            }
            catch (Exception)
            {
                return new SortedSet<string>();
            }
        }

        private void AddLinksToQueue(IEnumerable<string> links)
        {
            foreach (var url in links)
            {
                url.FormatUrl();

                if (SpiderData.Queue.Contains(url) || SpiderData.Crawled.Contains(url))
                    continue;
                if (DomainName != url.GetDomainName())
                    continue;

                SpiderData.Queue.Enqueue(url);
            }
        }

        private void CreateDocument()
        {
            var rootObject = Task.Run(() => _spiderRepository.GetAllAsync()).Result;
            var dados = rootObject.rows.FirstOrDefault(d => d.doc.BaseUrl.GetDomainName() == SpiderData.BaseUrl.GetDomainName());

            if (dados == null)
                Task.Run(async () => await _spiderRepository.CreateAsync(SpiderData)).Wait();
            else
                SpiderData.Update(dados.doc);
        }

        private async Task UpdateDocuments()
        {
            await _spiderRepository.UpdateAsync(SpiderData);
        }
    }
}
