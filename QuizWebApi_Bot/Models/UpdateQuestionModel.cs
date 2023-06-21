namespace QuizWebApi_Bot.Models;

public class UpdateQuestionModel
{
    public string? QuestionText { get; set; }
    public string? Description { get; set; }
    public List<string>? Choices { get; set; }
    public string? CorrectAnswer { get; set; }
}

