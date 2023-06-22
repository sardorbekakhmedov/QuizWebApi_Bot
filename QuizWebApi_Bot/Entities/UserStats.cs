using MongoDB.Bson.Serialization.Attributes;

namespace QuizWebApi_Bot.Entities;

public class UserStats
{
    [BsonId]
    public long UserId { get; set; }

    public required string UserName { get; set; }
    public int TotalQuestionsSent { get; set; } // => QuestionCollection.Count;
    public int TotalQuestionsAnswered { get; set; }
    public int CorrectlyAnswered { get; set; }

  //  public List<QuestionCollection> QuestionCollection { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
