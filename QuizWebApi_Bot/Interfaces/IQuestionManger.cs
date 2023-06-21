using QuizWebApi_Bot.Entities;
using QuizWebApi_Bot.Models;

namespace QuizWebApi_Bot.Interfaces;

public interface IQuestionManger
{
    QuestionModel MapToQuestionModel(Question question);
}