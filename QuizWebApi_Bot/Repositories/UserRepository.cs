using MongoDB.Driver;
using QuizWebApi_Bot.Entities;
using QuizWebApi_Bot.Interfaces;

namespace QuizWebApi_Bot.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserStats> _userStats;

    public UserRepository(IQuestionManger questionManger)
    {
        var client = new MongoClient("mongodb://root:password@localhost:27017");
        var db = client.GetDatabase("quizwebapi_db");
        _userStats = db.GetCollection<UserStats>("user_stats");
    }


    public async Task AddUserAsync(long userChatId, string userNAme)
    {
        var users = await (await _userStats.FindAsync(_ => true)).ToListAsync();
        var user = users.FirstOrDefault(user => user.UserId == userChatId);

        if (user is null)
        {
            var newUser = new UserStats
            {
                UserId = userChatId,
                UserName = userNAme,
                TotalQuestionsAnswered = 0,
                CorrectlyAnswered = 0,
            };

            await _userStats.InsertOneAsync(newUser);
        }
    }
/*
    public async Task AddSentUserQuestionCollectionAsync(UserStats user, Guid questionId, bool isAnswer)
    {
        user.QuestionCollection.Add(new QuestionCollection
        {
            QuestionId = questionId,
            IsAnswer = isAnswer
        });

        user.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<UserStats>.Filter.Eq(u => u.UserId, user.UserId);

        await _userStats.ReplaceOneAsync(filter, user);
    }

    public async Task DeleteSentUserQuestionCollectionAsync(UserStats user, Guid questionId)
    {
        if (user.QuestionCollection.Any(i => i.QuestionId == questionId))
        {
            var question = user.QuestionCollection.First(i => i.QuestionId == questionId);
            user.QuestionCollection.Remove(question);
        }

        var filter = Builders<UserStats>.Filter.Eq(u => u.UserId, user.UserId);

        user.UpdatedAt = DateTime.UtcNow;
        await _userStats.ReplaceOneAsync(filter, user);
    }
*/
    public async Task<string> GetUserStatsAsync(long userChatId)
    {
        var user = await GetUserAsync(userChatId);

       return $"Username:  {user.UserName}" +
              $"\n\nTotal correct answered count: {user.CorrectlyAnswered}" +
              $"\nTotal question sent count:  {user.TotalQuestionsSent}" +
              $"\nTotal questions Answered {user.TotalQuestionsAnswered} ";
    }

    public async Task IncrementAnswerAsync(long userChatId)
    {
        var user = await GetUserAsync(userChatId);

        user.TotalQuestionsAnswered++;
        user.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<UserStats>.Filter.Eq(u => u.UserId, userChatId);

        await _userStats.ReplaceOneAsync(filter, user);
    }

    public async Task IncrementCorrectAnswerAsync(long userChatId)
    {
        var user = await GetUserAsync(userChatId);

        user.CorrectlyAnswered++;
        user.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<UserStats>.Filter.Eq(u => u.UserId, userChatId);

        await _userStats.ReplaceOneAsync(filter, user);
    }

    public async Task IncrementSentQuestionAsync(long userChatId)
    {
        var user = await GetUserAsync(userChatId);

        user.TotalQuestionsSent++;
        user.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<UserStats>.Filter.Eq(u => u.UserId, userChatId);

        await _userStats.ReplaceOneAsync(filter, user);
    }

    public async Task<List<UserStats>> GetAllUsersAsync()
    {
       return await (await _userStats.FindAsync(_ => true)).ToListAsync();
    }


    public async Task<UserStats> GetUserAsync(long userChatId)
    {
        var users = await (await _userStats.FindAsync(_ => true)).ToListAsync();

        var user = users.FirstOrDefault(user => user.UserId == userChatId);

        return user ?? throw new Exception("User not found!");
    }
}