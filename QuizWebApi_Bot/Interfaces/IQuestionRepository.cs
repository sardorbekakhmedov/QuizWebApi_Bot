using QuizWebApi_Bot.Entities;
using QuizWebApi_Bot.Models;

namespace QuizWebApi_Bot.Interfaces;

public interface IQuestionRepository
{
    Task<Guid> AddQuestionAsync(CreateQuestionModel model);
    Task<QuestionModel> AddImageAsync(ImageModel model);
    Task<Question?> GetQuestionByIdAsync(Guid questionId);
    Task<QuestionModel> UpdateQuestionAsync(Guid questionId, UpdateQuestionModel model);
    Task DeleteQuestionAsync(Guid questionId);
    Task<List<QuestionModel>> GetQuestionsAsync();
    Task<QuestionModel> GetRandomQuestionAsync();
}