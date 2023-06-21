namespace QuizWebApi_Bot.Models;

public class QuestionModel
{
    public Guid Id { get; set; }
    public required string QuestionText { get; set; }
    public string? Description { get; set; }
    public required List<string> Choices { get; set; }
    public required string CorrectAnswer { get; set; }
    public string? ImagePath { get; set; }
}