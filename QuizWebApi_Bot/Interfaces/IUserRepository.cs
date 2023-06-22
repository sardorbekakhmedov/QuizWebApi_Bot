using QuizWebApi_Bot.Entities;

namespace QuizWebApi_Bot.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(long userChatId, string userNAme);
    Task<List<UserStats>> GetAllUsersAsync();
    Task<UserStats> GetUserAsync(long userChatId);
    Task<string> GetUserStatsAsync(long userChatId);
    Task IncrementAnswerAsync(long userChatId);
    Task IncrementCorrectAnswerAsync(long userChatId);
    Task IncrementSentQuestionAsync(long userChatId);

    //  Task AddSentUserQuestionCollectionAsync(UserStats user, Guid questionId, bool isAnswer);
    //   Task DeleteSentUserQuestionCollectionAsync(UserStats user, Guid questionId);
}