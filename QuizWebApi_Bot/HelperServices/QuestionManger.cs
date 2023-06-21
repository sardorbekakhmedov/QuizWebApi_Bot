using QuizWebApi_Bot.Entities;
using QuizWebApi_Bot.Interfaces;
using QuizWebApi_Bot.Models;

namespace QuizWebApi_Bot.HelperServices;

public class QuestionManger : IQuestionManger
{
    public QuestionModel MapToQuestionModel(Question question)
    {
        return new QuestionModel
        {
            Id = question.Id,
            QuestionText = question.QuestionText,
            Description = question.Description,
            Choices = question.Choices,
            CorrectAnswer = question.CorrectAnswer,
            ImagePath = question.ImagePath
        };
    }
}