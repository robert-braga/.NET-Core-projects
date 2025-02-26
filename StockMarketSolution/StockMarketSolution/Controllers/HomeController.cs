using Microsoft.AspNetCore.Mvc;

namespace StockMarketSolution.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
