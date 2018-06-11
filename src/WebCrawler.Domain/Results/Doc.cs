namespace WebCrawler.Domain.Results
{
    public class Doc
    {
        public string _id { get; set; }
        public string _rev { get; set; }
        public string BaseUrl { get; set; }
        public string[] Queue { get; set; }
        public string[] Crawled { get; set; }
    }
}