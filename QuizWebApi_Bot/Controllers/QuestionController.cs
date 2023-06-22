using Microsoft.AspNetCore.Mvc;
using QuizWebApi_Bot.Interfaces;
using QuizWebApi_Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuizWebApi_Bot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserRepository _userRepository;

    public QuestionController(IQuestionRepository questionRepository, IUserRepository userRepository)
    {
        _questionRepository = questionRepository;
        _userRepository = userRepository;
    }

    [HttpPost("bot")]
    public async Task<IActionResult> PostUpdate([FromBody] Update update)
    {
        var bot = new TelegramBotClient("5674695715:AAEGpPEyu_tUbeJp_C4slf89laNWGq9PQM0");

        var (messageText, firstName, messageId, chatId, isSuccess) = GetMessage(update);

        if (!isSuccess)
            return BadRequest();

        if (messageText == "/start")
        {
            await bot.SendTextMessageAsync(chatId, "🖐 Assalomu alekum, \nSizga har soatda bittadan savol yuboriladi");
            await _userRepository.AddUserAsync(chatId, firstName ?? "No name");
        }
        else if (messageText is not null && messageText.StartsWith("!_?answer?_!"))
        {
            
            var user = await _userRepository.GetUserAsync(chatId);
            string[] array = messageText.Split(',');

            var questionId = Guid.Parse(array[1]);
            var answerData = array[2];

            await _userRepository.IncrementAnswerAsync(chatId);

          //  await _userRepository.DeleteSentUserQuestionCollectionAsync(user, questionId);
            var question = await _questionRepository.GetQuestionByIdAsync(questionId);

            if (question != null)
            {
                if (question.CorrectAnswer == answerData)
                {
                    await bot.SendTextMessageAsync(chatId, "Qoyil 👍  javobingiz to'g'ri  ✅");
                    await _userRepository.IncrementCorrectAnswerAsync(chatId);
                }
                else
                {
                    await bot.SendTextMessageAsync(chatId, $"Afsus 🥵  javob no to'g'ri  ❌  " +
                                                           $"\n\n To'gri javob:  {question.CorrectAnswer}," +
                                                           $"\n\nJavob ta'rifi:  {question.Description}");
                }
            }
        }
        else 
            await bot.SendTextMessageAsync(chatId, "$\"☢  No malum buyruq kiritildi!");

        return Ok();
    }

    private static (string? messageText, string? firstName, int messageId, long chatId, bool isSuccess) GetMessage(Update update)
    {
        if (update.Type == UpdateType.Message)
            return (update.Message?.Text ?? "No text", update.Message?.From?.FirstName ?? "No Name", update.Message?.MessageId ?? -1, update.Message?.Chat.Id ?? -1, true);

        if (update is { Type: UpdateType.CallbackQuery, CallbackQuery: { Data: not null, Message: not null } })
            return (update.CallbackQuery.Data, update.CallbackQuery.From?.FirstName ?? "No Name", update.CallbackQuery.Message.MessageId, update.CallbackQuery.Message.Chat.Id, true);

        return (null, null, -1, -1, false);
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

    [HttpGet("get_users")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userRepository.GetAllUsersAsync());
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