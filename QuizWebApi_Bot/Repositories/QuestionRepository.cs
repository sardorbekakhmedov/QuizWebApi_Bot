using MongoDB.Driver;
using QuizWebApi_Bot.Entities;
using QuizWebApi_Bot.Interfaces;
using QuizWebApi_Bot.Models;

namespace QuizWebApi_Bot.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly IQuestionManger _questionManger;
    private readonly IFileManager _fileManager;
    private readonly IMongoCollection<Question> _questionCollection;

    public QuestionRepository(IQuestionManger questionManger, IFileManager fileManager)
    {
        _questionManger = questionManger;
        _fileManager = fileManager;
        var client = new MongoClient("mongodb://root:password@localhost:27017");
        var db = client.GetDatabase("quizwebapi_db");
        _questionCollection = db.GetCollection<Question>("questions");
    }

    public async Task<Guid> AddQuestionAsync(CreateQuestionModel model)
    {
        var question = new Question
        {
            Id = Guid.NewGuid(),
            QuestionText = model.QuestionText,
            Description = model.Description,
            Choices = model.Choices,
            CorrectAnswer = model.CorrectAnswer,
        };

        await _questionCollection.InsertOneAsync(question);

        return question.Id;
    }

    public async Task<QuestionModel> UpdateQuestionAsync(Guid questionId, UpdateQuestionModel model)
    {
        var question = await GetQuestionByIdAsync(questionId);

        if (question is null)
            throw new Exception("Question not found!");

        question.QuestionText = model.QuestionText ?? question.QuestionText;
        question.Description = model.Description ?? question.Description;
        question.CorrectAnswer = model.CorrectAnswer ?? question.CorrectAnswer;
        question.Choices = model.Choices ?? question.Choices;

        var filter = Builders<Question>.Filter.Eq(i => i.Id, questionId);
        await _questionCollection.ReplaceOneAsync(filter, question);

        return _questionManger.MapToQuestionModel(question);
    }


    public async Task<QuestionModel> AddImageAsync(ImageModel model)
    {
        var question = await GetQuestionByIdAsync(model.QuestionId);

        if (question is null)
            throw new Exception("Question not found!");

        question.ImagePath = await _fileManager.SaveFileToWwwrootAsync(model.ImageFile, "QuestionImages");

        var filter = Builders<Question>.Filter.Eq(i => i.Id, question.Id);
        await _questionCollection.ReplaceOneAsync(filter, question);

        return _questionManger.MapToQuestionModel(question);
    }

    public async Task<List<QuestionModel>> GetQuestions()
    {
        var questions = await (await _questionCollection.FindAsync(_ => true)).ToListAsync();

        return questions.Select(item => _questionManger.MapToQuestionModel(item)).ToList();
    }

    public async Task DeleteQuestionAsync(Guid questionId)
    {
        var filter = Builders<Question>.Filter.Eq(i => i.Id, questionId);

        await _questionCollection.DeleteOneAsync(filter);
    }

    public async Task<Question?> GetQuestionByIdAsync(Guid questionId)
    {
        return await (await _questionCollection.FindAsync(qs => qs.Id == questionId)).FirstOrDefaultAsync();
    }
}