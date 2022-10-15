using Microsoft.AspNetCore.Mvc;
using SimpleSearchUI.Models;
using SimpleSearchUI.Models.Nest;
using System.Diagnostics;

namespace SimpleSearchUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ElasticsearchApi _elasticsearchApi;

        public HomeController(ILogger<HomeController> logger, ElasticsearchApi elasticsearch)
        {
            _logger = logger;
            _elasticsearchApi = elasticsearch;
        }

        public IActionResult Index(string key = "", int? pageindex = null, string indexname = null)
        {
            if (!string.IsNullOrWhiteSpace(key)&& !string.IsNullOrWhiteSpace(indexname))
            {
                if (pageindex == null) pageindex = 1;

                ViewBag.Result = _elasticsearchApi.SearchData(key, pageindex.Value, indexname);
            }

            ViewBag.IndexNames = _elasticsearchApi.GetAllIndexName();

            ViewBag.CurrentIndexName = indexname;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}