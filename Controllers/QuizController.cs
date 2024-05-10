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
using System.Collections.Generic;

namespace Quiz.Controllers
{
    public class QuizController : Controller
        
    {
     
        private readonly IQuizRepository  _quizRepository;
        private readonly IStartedQuizRepository _StartedQuizRepository;
        private readonly IHttpContextAccessor _context;
        private readonly IUserRepository _userRepository;

        public QuizController(IQuizRepository quizRepository, IUserRepository userRepository, IHttpContextAccessor context, IStartedQuizRepository StartedQuizRepository)
        {
            
            _quizRepository = quizRepository;
            _userRepository= userRepository;
              _context = context;
            _StartedQuizRepository = StartedQuizRepository;

        }

        public async Task<IActionResult> QuizIsStarted(string QuizCode)
        {
            StartedQuizTeacher startedQuizTeacher = await _StartedQuizRepository.GetStartedQuizByCodeQuiz(QuizCode);
            startedQuizTeacher.IsStarted = true;
            _StartedQuizRepository.UpdateStartedQuizTeacher(startedQuizTeacher);
           
            Dictionary<User, Dictionary<int,bool>> userScores = new Dictionary<User, Dictionary<int, bool>>();
            var students = await _StartedQuizRepository.ListStudentQuiz(startedQuizTeacher.IdStartedQuizTeacher);
            foreach (StartedQuizStudent s in students)
            {
                if (!s.IsRefused)
                {
                    Dictionary<int, bool> quizScores = new Dictionary<int, bool>();
                    quizScores.Add(s.Score, s.terminate);
                    User user = await _userRepository.GetByIdAsync(s.UserId.Value);
                    userScores.Add(user, quizScores);
                }
               
              
            }
            ViewBag.userScores = userScores;
            ViewBag.QuizCode = QuizCode;
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
                     
                  ViewBag.error = "We apologize, but the quiz has already started.";

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
                StartedQuizStudent startedQuizStudentNew1 = await _StartedQuizRepository.GetStartedQuizStudentAsync(user.UserId, startedQuizTeacher.IdStartedQuizTeacher);
                if (startedQuizStudentNew1.started)
                {
                    ViewBag.error = "It's not possible, you have already passed this quiz.";

                    return View("RejoindreQuiz");
                }
                if (startedQuizStudentNew1.IsRefused)
                {
                    ViewBag.error = "you are refused to joined to this Quiz ";

                    return View("RejoindreQuiz");
                }

                var isStarted = startedQuizTeacher.IsStarted;
                if (isStarted)
                {
                    StartedQuizStudent startedQuizStudentNew = await _StartedQuizRepository.GetStartedQuizStudentAsync(user.UserId, startedQuizTeacher.IdStartedQuizTeacher);
                    startedQuizStudentNew.started = true;
                    _StartedQuizRepository.Save();
                    Models.Quiz quiz = await _quizRepository.GetById(startedQuizTeacher.QuizId.Value);
                    ViewBag.idQ = startedQuizTeacher.QuizId.Value;
                    ViewBag.quiz = quiz;
                 
                    ViewBag.startedQuizStudentNewId = startedQuizStudentNew.Id;
                    ViewBag.codeQuiz = CodeQuiz;
                    return View("QuestionPage");
                    
                }

                return View(startedQuizTeacher); }
           
            
        }
        [HttpPost]
        public async Task<IActionResult> SendResponses(Response res ,int StudentQuizId, int QuizId,string CodeQuiz)
        {
           
            StartedQuizStudent studentQuizStudent = await  _StartedQuizRepository.GetStartedQuizStudentAsyncById(StudentQuizId);
            studentQuizStudent.terminate = true;
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
            ViewBag.bestMark = quiz.Questions.Count;
            ViewBag.codeQuiz= CodeQuiz;
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
                        ViewBag.myquiz = quiz;
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
                                if (!s.IsRefused)
                                {
                                    User userA = await _userRepository.GetByIdAsync(s.UserId.Value);
                                    listeUser.Add(userA);
                                }
                                   
                              
                               

                            }
                            ViewBag.ListeStudent = listeUser;
                        



                        }
                        else
                        {
                          
                            ViewBag.ListeStudent = null;
                        }
                        ViewBag.iCodeQuiz = uniqueIdString;
                     
                       
                        ViewBag.idQuiz = idQuiz;
                        ViewBag.idTeacher = startedQuizTeachernew.IdStartedQuizTeacher;


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
        [HttpPost]
        public async Task<IActionResult> DeleteParticipant(int TeacherQuizStartedId ,int UserId , string codeQuiz ,int IdQuiz)
        {
         StartedQuizStudent s = await   _StartedQuizRepository.GetStartedQuizStudentAsync(UserId, TeacherQuizStartedId);
            s.IsRefused = true;
            _StartedQuizRepository.Save();

            return  RedirectToAction("StartQuizByTeacher", new { idQuiz = IdQuiz, uniqueIdString = codeQuiz });
            ;
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

        public async Task<IActionResult> PassedQuizzes()
        {
            var currentUser = _context.HttpContext.Session.GetString("currentUser");
            User CurrentrUser = JsonConvert.DeserializeObject<User>(currentUser);
            List<StartedQuizTeacher> ListStartedQuizTeacher = await   _StartedQuizRepository.GetListStartedTeacher(CurrentrUser.UserId);

            ViewBag.ListStartedQuizTeacher=ListStartedQuizTeacher;

            return View();
        }
        public async Task<IActionResult> QuizzesAsync()
        {
            IEnumerable<Models.Quiz> list = await _quizRepository.GetAll();
            ViewBag.Quizzes = list;

            return View();
        }
        public async Task<IActionResult> ListPassedStudent(int idStartedQuiz)
        {
            Dictionary<User, StartedQuizStudent> userScores = new Dictionary<User, StartedQuizStudent>();
            var students = await _StartedQuizRepository.ListStudentQuiz(idStartedQuiz);
            foreach (var s in students)
            {
                if (!s.IsRefused)
                {
                    User userA = await _userRepository.GetByIdAsync(s.UserId.Value);
                    userScores.Add(userA,s);
                }




            }
            Dictionary<User, StartedQuizStudent> orderedUserScores = userScores
                .OrderByDescending(kv => kv.Value.Score)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            ViewBag.userScores = orderedUserScores;
            return View();

        }

    }
}
