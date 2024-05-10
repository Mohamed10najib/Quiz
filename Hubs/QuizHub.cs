using Microsoft.AspNetCore.SignalR;

namespace Quiz.Hubs
{
    public sealed class QuizHub : Hub
    {
        public async Task StudentJoined(string CodeQuiz)
        {
          
            await Clients.All.SendAsync("StudentJoined", CodeQuiz);
        }
        public async Task TechearStartedQuiz(string CodeQuiz)
        {

            await Clients.All.SendAsync("TechearStartedQuiz", CodeQuiz);
        }
        public async Task StudentEndQuiz(string CodeQuiz)
        {

            await Clients.All.SendAsync("StudentEndQuiz", CodeQuiz);
        }
    }
}