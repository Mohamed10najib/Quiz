using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Quiz.Hubs;
using Quiz.Models;
using System.Diagnostics;

namespace Quiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<QuizHub> _hubContext;

        public HomeController(ILogger<HomeController> logger,IHubContext<QuizHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
