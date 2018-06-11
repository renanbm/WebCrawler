namespace WebCrawler.Domain.Results
{
    public class Rootobject
    {
        public int total_rows { get; set; }
        public int offset { get; set; }
        public Row[] rows { get; set; }
    }
}