using System;
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
        public JsonResult Post(string baseUrl)
        {
            try
            {
                var retorno = _spiderAppService.Crawl(baseUrl);
                return Json(new { success = true, retorno, validacao = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, validacao = true, errorMessage = ex.Message });
            }
        }
    }
}