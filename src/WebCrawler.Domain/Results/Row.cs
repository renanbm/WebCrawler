namespace WebCrawler.Domain.Results
{
    public class Row
    {
        public string id { get; set; }
        public string key { get; set; }
        public Value value { get; set; }
        public Doc doc { get; set; }
    }
}