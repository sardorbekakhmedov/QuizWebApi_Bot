using MongoDB.Bson.Serialization.Attributes;

namespace QuizWebApi_Bot.Entities;

public class User
{
    [BsonId]
    public long UserId { get; set; }
    public int TotalQuestionsSent { get; set; }
    public int TotalQuestionsAnswered { get; set; }
    public int CorrectlyAnswered { get; set; }
}