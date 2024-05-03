using Microsoft.AspNetCore.SignalR;

namespace Quiz.Hubs
{
    public class QuizHub : Hub
    {
        public async Task StudentJoinedQuiz(string quizId, string studentName)
        {
            // Broadcast to all connected clients that a student has joined the quiz
            await Clients.All.SendAsync("StudentJoinedQuiz", quizId, studentName);
        }
    }
}
