namespace QuizWebApi_Bot.Models;

public class UserModel
{
    public long UserId { get; set; }

    public required string UserName { get; set; }
    public int TotalQuestionsSent { get; set; } 
    public int TotalQuestionsAnswered { get; set; }
    public int CorrectlyAnswered { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}