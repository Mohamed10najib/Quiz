using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Quiz.Data;
using Quiz.Hubs;
using Quiz.interfaces;
using Quiz.Models;
using Quiz.Repository;

namespace Quiz.Controllers
{
    public class QuizController : Controller
        
    {
        private readonly IHubContext<QuizHub> _hubContext;
        private readonly IQuizRepository  _quizRepository;
        private readonly IStartedQuizRepository _StartedQuizRepository;
        private readonly IHttpContextAccessor _context;
        private readonly IUserRepository _userRepository;

        public QuizController(IHubContext<QuizHub> hubContext,IQuizRepository quizRepository, IUserRepository userRepository, IHttpContextAccessor context, IStartedQuizRepository StartedQuizRepository)
        {
            _hubContext = hubContext;
            _quizRepository = quizRepository;
            _userRepository= userRepository;
              _context = context;
            _StartedQuizRepository = StartedQuizRepository;

        }

        public async Task<IActionResult> QuizIsStarted(string QuizCode)
        {
            await _hubContext.Clients.All.SendAsync("StudentJoinedQuiz", "mohamed", "najib");
            StartedQuizTeacher startedQuizTeacher = await _StartedQuizRepository.GetStartedQuizByCodeQuiz(QuizCode);
            startedQuizTeacher.IsStarted = true;
            _StartedQuizRepository.UpdateStartedQuizTeacher(startedQuizTeacher);
           
            Dictionary<User, int> userScores = new Dictionary<User, int>();
            var students = await _StartedQuizRepository.ListStudentQuiz(startedQuizTeacher.IdStartedQuizTeacher);
            foreach (StartedQuizStudent s in students)
            {
                User user=  await _userRepository.GetByIdAsync(s.UserId.Value);
                userScores.Add(user, s.Score);
              
            }
            ViewBag.userScores = userScores;
            return View();
        }
        public async Task<IActionResult> JoinQuizApreCode(string CodeQuiz)
        {
            StartedQuizTeacher startedQuizTeacher = await _StartedQuizRepository.GetStartedQuizByCodeQuiz(CodeQuiz);
            if(startedQuizTeacher == null ) {

               
                ViewBag.error = "The quiz has not started yet.";
                return View("RejoindreQuiz"); }
            else {
                if (startedQuizTeacher.IsTerminated) {
                     
                  ViewBag.QuizWasStarted = "We apologize, but the quiz has already started.";

                    return View("RejoindreQuiz");
                }
                string userString = _context.HttpContext.Session.GetString("currentUser");
                Models.User user = JsonConvert.DeserializeObject<Models.User>(userString);
                bool isExist = await _StartedQuizRepository.IsJoinStudent(user.UserId,CodeQuiz);
                if (!isExist)
                {
                    StartedQuizStudent startedQuizStudent = new StartedQuizStudent(user.UserId, startedQuizTeacher.TeacherId);
                    if (startedQuizTeacher.StartedQuizStudents == null)
                    {
                        
                        startedQuizTeacher.StartedQuizStudents = new List<StartedQuizStudent>();
                    }
                    startedQuizTeacher.StartedQuizStudents.Add(startedQuizStudent);

                    _StartedQuizRepository.AddStartedStudent(startedQuizStudent);
                   
                     _StartedQuizRepository.UpdateStartedQuizTeacher(startedQuizTeacher);


                }
                var isStarted = startedQuizTeacher.IsStarted;
                if (isStarted)
                {
                    Models.Quiz quiz = await _quizRepository.GetById(startedQuizTeacher.QuizId.Value);
                    ViewBag.idQ = startedQuizTeacher.QuizId.Value;
                    ViewBag.quiz = quiz;
                    StartedQuizStudent startedQuizStudentNew = await _StartedQuizRepository.GetStartedQuizStudentAsync(user.UserId, startedQuizTeacher.IdStartedQuizTeacher);


                    ViewBag.startedQuizStudentNewId = startedQuizStudentNew.Id;
                    
                    return View("QuestionPage");
                }

                return View(startedQuizTeacher); }
           
            
        }
        [HttpPost]
        public async Task<IActionResult> SendResponses(Response res ,int StudentQuizId, int QuizId)
        {
           
            StartedQuizStudent studentQuizStudent = await  _StartedQuizRepository.GetStartedQuizStudentAsyncById(StudentQuizId);
            Models.Quiz quiz = await _quizRepository.GetById(QuizId);
            List<Question> ListeQuestions = quiz.Questions.ToList();
            int scoreN = 0;
            if (res != null && res.Responses != null)
            {
                
                for (int i = 0; i < res.Responses.Count && i < quiz.Questions.Count; i++)
                {
                    if (res.Responses[i].Count > 1) {

                        if (ListeQuestions[i].Response == res.Responses[i][1])
                        {
                            scoreN++;
                        }


                    }
                    
                }
            }
                ViewBag.scoreN = scoreN;
            studentQuizStudent.Score= scoreN;
             _StartedQuizRepository.Save();
            return View(res);
        }
        public IActionResult RejoindreQuiz()
        {

            return View();
        }
        public async Task<IActionResult> StartQuizByTeacher(int idQuiz, string uniqueIdString)
        {
            string userString = _context.HttpContext.Session.GetString("currentUser");
            if (userString != null)
            {
                Models.User user = JsonConvert.DeserializeObject<Models.User>(userString);
                if (user != null)
                {
                    StartedQuizTeacher startedQuizTeachernew;
                    Models.Quiz quiz = await _quizRepository.GetById(idQuiz);
                    if (quiz != null)
                    {
                        bool isExist = await _StartedQuizRepository.IsExistStartedQuizByCodeQuiz(uniqueIdString);
                        if (!isExist) {
                             startedQuizTeachernew = new StartedQuizTeacher(user.UserId, idQuiz, uniqueIdString);
                            _StartedQuizRepository.AddStartedTeacher(startedQuizTeachernew);
                        }
                        else {  startedQuizTeachernew =  await _StartedQuizRepository.GetStartedQuizByCodeQuiz(uniqueIdString);
                  }


                        if (startedQuizTeachernew != null)
                        {
                            List<User> listeUser= new List<User>();
                            var students = await _StartedQuizRepository.ListStudentQuiz(startedQuizTeachernew.IdStartedQuizTeacher);
                            foreach(var s in students)
                            {
                                User userA = await _userRepository.GetByIdAsync(s.UserId.Value);
                                listeUser.Add(userA);

                            }
                            ViewBag.ListeStudent = listeUser;




                        }
                        else
                        {
                          
                            ViewBag.ListeStudent = null;
                        }
                        ViewBag.iCodeQuiz = uniqueIdString;
                        ViewBag.idQuiz = idQuiz;

                        return View();
                    }
                    else
                    {
                        // Handle case when quiz is null
                        return NotFound(); // Or return appropriate error response
                    }
                }
                else
                {
                    // Handle case when user is null after deserialization
                    return BadRequest(); // Or return appropriate error response
                }
            }
            else
            {
                // Handle case when userString is null
                return BadRequest(); // Or return appropriate error response
            }
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
