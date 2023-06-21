using MongoDB.Bson.Serialization.Attributes;

namespace QuizWebApi_Bot.Entities;

public class Question
{
    [BsonId]
    public Guid Id { get; set; }
    public required string QuestionText { get; set; }
    public string? Description { get; set; }
    public required List<string> Choices { get; set; }
    public required string CorrectAnswer { get; set; }
    public string? ImagePath { get; set; }
}