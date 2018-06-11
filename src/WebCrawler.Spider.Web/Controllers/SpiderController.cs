using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.Spider.Application.Interfaces;

namespace WebCrawler.Spider.Web.Controllers
{
    public class SpiderController : Controller
    {
        private readonly ISpiderAppService _spiderAppService;

        public SpiderController(ISpiderAppService spiderAppService)
        {
            _spiderAppService = spiderAppService;
        }

        [HttpPost]
        public IEnumerable<string> Post(string baseUrl)
        {
            try
            {
                return _spiderAppService.Crawl(baseUrl);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}