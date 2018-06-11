using Microsoft.AspNetCore.Mvc;

namespace WebCrawler.Spider.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
