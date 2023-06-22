using Microsoft.AspNetCore.Mvc;
using QuizWebApi_Bot.Interfaces;
using QuizWebApi_Bot.Models;

namespace QuizWebApi_Bot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionController(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddQuestionAsync(CreateQuestionModel model)
    {
        var question = await _questionRepository.AddQuestionAsync(model);
        return Ok(question);
    }

    [HttpPost("add_image")]
    public async Task<IActionResult> AddImageAsync([FromForm] ImageModel model)
    {
        var questionModel = await _questionRepository.AddImageAsync(model);
        return Ok(questionModel);
    }

    [HttpPut("{questionId}")]
    public async Task<IActionResult> UpdateQuestionAsync(Guid questionId, UpdateQuestionModel model)
    {
        return Ok(await _questionRepository.UpdateQuestionAsync(questionId, model));
    }


    [HttpGet("random_question")]
    public async Task<IActionResult> GetRandomQuestionAsync()
    {
        return Ok(await _questionRepository.GetRandomQuestionAsync());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuestionsAsync()
    {
        return Ok(await _questionRepository.GetQuestionsAsync());
    }

    [HttpGet("{questionId}")]
    public async Task<IActionResult> GetQuestionByIdAsync(Guid questionId)
    {
        return Ok(await _questionRepository.GetQuestionByIdAsync(questionId));
    }

    [HttpDelete("{questionId}")]
    public async Task<IActionResult> DeleteQuestionAsync(Guid questionId)
    {
        await _questionRepository.DeleteQuestionAsync(questionId);
        return Ok();
    }
}