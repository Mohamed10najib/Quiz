using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiz.Data;
using Quiz.interfaces;
using Quiz.Models;
using Quiz.Repository;

namespace Quiz.Controllers
{
    public class QuizController : Controller
        
    {
        private readonly IQuizRepository  _quizRepository;
        private readonly IHttpContextAccessor _context;

        public QuizController(IQuizRepository quizRepository, IHttpContextAccessor context)
        {

            _quizRepository = quizRepository;
            _context = context;

        }
        public IActionResult StartQuizByTeacher()
        {


            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            _context.HttpContext.Session.SetString("Quest", "false");

            return View();
        }
        [HttpPost]
        public IActionResult Create(Models.Quiz quiz)
        {
            
            var currentUser = _context.HttpContext.Session.GetString("currentUser");
            User CurrentrUser = JsonConvert.DeserializeObject<User>(currentUser);
            quiz.UserId=CurrentrUser.UserId;
            Console.WriteLine(CurrentrUser.UserId + " " + CurrentrUser.Username);

            // Set quiz for each question
          

            // Logging
            Console.WriteLine($"Number of questions submitted: {quiz.Questions?.Count}");

            if (!ModelState.IsValid)
            {

                var errors = ModelState.Where(ms => ms.Value.Errors.Any())
                           .Select(ms => new { Field = ms.Key, Errors = ms.Value.Errors.Select(e => e.ErrorMessage) })
                           .ToList();

                // Print errors to the console
                foreach (var error in errors)
                {
                    foreach (var errorMessage in error.Errors)
                    {
                        Console.WriteLine($"Field: {error.Field}, Error: {errorMessage}");
                    }
                }
                return View(quiz);
            }
           
            //_questionRepository.AddAll((List<Question>)quiz.Questions);
            _quizRepository.Add(quiz);
            return RedirectToAction("Create","Question",quiz);
        }
        public async Task<IActionResult> QuizzesAsync()
        {
            IEnumerable<Models.Quiz> list = await _quizRepository.GetAll();
            ViewBag.Quizzes = list;

            return View();
        }

    }
}
