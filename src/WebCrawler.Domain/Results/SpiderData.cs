using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WebCrawler.Domain.Results
{
    public class SpiderData
    {
        public SpiderData(string baseUrl)
        {
            BaseUrl = baseUrl;
            Queue = new ConcurrentQueue<string>();
            Crawled = new SortedSet<string>();
            Queue.Enqueue(baseUrl);
        }

        public string id { get; set; }
        public string rev { get; set; }
        public string BaseUrl { get; }
        public ConcurrentQueue<string> Queue { get; private set; }
        public ISet<string> Crawled { get; private set; }

        public void Update(Doc dados)
        {
            id = dados._id;
            rev = dados._rev;
            Queue = new ConcurrentQueue<string>(dados.Queue);
            Crawled = new SortedSet<string>(dados.Crawled);
        }
    }
}