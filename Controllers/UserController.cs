using Microsoft.AspNetCore.Mvc;
using Quiz.interfaces;
using Quiz.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Quiz.Controllers
{

    public class UserController : Controller
        
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository, IHttpContextAccessor context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool exist=_userRepository.checkUser(user);
            if (!exist)
            {
                ViewBag.ErrorMessage = "Invalid email address or password.";
                return View(user);
            }
            User user2 = await _userRepository.GetByEmailAndPasswordAsync(user.Username,user.Password);
            string objetEnString = JsonConvert.SerializeObject(user2);

            _context.HttpContext.Session.SetString("currentUser", objetEnString);
            return RedirectToAction("Index", "Quiz");
        }
        public IActionResult Registrer()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult>  Registrer(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

             _userRepository.Add(user);

            return RedirectToAction("Index");
        }
    }
}
