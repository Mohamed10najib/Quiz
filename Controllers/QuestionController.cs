using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiz.interfaces;
using Quiz.Models;

namespace Quiz.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IHttpContextAccessor _context;

        public QuestionController(IQuestionRepository questionRepository, IHttpContextAccessor context)
        {
            _context = context;

            _questionRepository = questionRepository;
        }
        public IActionResult Create(Models.Quiz quiz)
        {
            string checkConnectionA = _context.HttpContext.Session.GetString("currentUser");
            if (checkConnectionA == null)
            {
                return RedirectToAction("index", "User");
            }

            TempData["nbrQ"] = quiz.NbrQuestion;
            TempData["quizId"] = quiz.QuizId;

            string objetEnString = JsonConvert.SerializeObject(quiz);
            _context.HttpContext.Session.SetString("quiz", objetEnString);

            return View();
        }
        [HttpPost]
        public IActionResult Create(Question question ,string resp)
        {
            string checkConnectionA = _context.HttpContext.Session.GetString("currentUser");
            if (checkConnectionA == null)
            {
                return RedirectToAction("index", "User");
            }
            var nbrQ = TempData["nbrQ"] as int?; // Retrieve nbrQ from TempData
            var quizId = TempData["quizId"] as int? ?? 0; // Retrieve nbrQ from TempData
            var q = _context.HttpContext.Session.GetString("quiz");
            Models.Quiz qui= JsonConvert.DeserializeObject<Models.Quiz>(q);
            question.QuizId = qui.QuizId;

            Console.WriteLine(quizId+"zz"+qui.QuizId);
            Console.WriteLine(nbrQ+ "hhh");

            if (nbrQ!=0)
            {
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
                    return View(question);
                }
                else
                {

                    nbrQ--;
                    Console.WriteLine("wahwah  "+nbrQ);

                    TempData["nbrQ"] = nbrQ;
                    ViewBag.message = nbrQ;
                    _questionRepository.Add(question);
                    if(nbrQ==0)
                        return RedirectToAction("index", "Quiz");

                    return View();
                }
            }
            
            return RedirectToAction("index","Quiz");

        }
    }
}
