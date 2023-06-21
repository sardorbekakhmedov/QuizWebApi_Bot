namespace QuizWebApi_Bot.Models;

public class ImageModel
{
    public Guid QuestionId { get; set; }
    public required IFormFile ImageFile { get; set; }
}