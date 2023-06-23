using Microsoft.AspNetCore.Mvc;
using QuizWebApi_Bot.Interfaces;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace QuizWebApi_Bot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BotController : ControllerBase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserRepository _userRepository;

    public BotController(IQuestionRepository questionRepository, IUserRepository userRepository)
    {
        _questionRepository = questionRepository;
        _userRepository = userRepository;
    }

    [HttpPost("update")]
    public async Task<IActionResult> PostUpdate([FromBody] Update update, CancellationToken cts)
    {
        var bot = new TelegramBotClient("5674695715:AAEGpPEyu_tUbeJp_C4slf89laNWGq9PQM0");

        var (messageText, firstName, messageId, chatId, isSuccess) = GetMessage(update);

        if (!isSuccess)
            return BadRequest();

        if (messageText == "/start")
        {
            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: "🖐 Assalomu alekum, \nSizga har soatda bittadan savol yuboriladi" +
                      "\n\nO'z natijalaringizni ko'rish uchun  /result  buyrug'ini yuboring" +
                      "\nSavol yuborishni to'xtatish uchun  /stopmessage  buyrug'ini yuboring," +
                      "\nSavol jo'natishni tiklash uchun  /startmessage  buyrug'ini yuboring",
                cancellationToken: cts);

            await _userRepository.AddUserAsync(chatId, firstName);
        }
        else if (messageText == "/startmessage")
        {
            await _userRepository.NoSentMessageAsync(chatId, false);

            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: " ⚠ Savol jo'natish tiklandi!",
                cancellationToken: cts);
        }
        else if (messageText == "/stopmessage")
        {
            await _userRepository.NoSentMessageAsync(chatId, true);

            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: " ⚠  Siz savol jo'natishni bekor qildingiz, \n" +
                      "Agarda siz qayta savol jo'natishni tiklamoqchi bo'lsangiz /startmessage buyrug'ini yuboring",
                cancellationToken: cts);
        }
        else if (messageText == "/result")
        {
            var result = await _userRepository.GetUserStatsAsync(chatId);
            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: result,
                cancellationToken: cts);
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
                    await bot.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Qoyil 👍  javobingiz to'g'ri  ✅",
                        cancellationToken: cts);

                    await _userRepository.IncrementCorrectAnswerAsync(chatId);
                }
                else
                {
                    await bot.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"Afsus 🥵  javob no to'g'ri  ❌  " +
                               $"\n\n To'g'ri javob:  {question.CorrectAnswer}," +
                               $"\n\nJavob tarifi:  {question.Description}",
                        cancellationToken: cts);
                }
            }
        }
        else
        {
            await bot.SendTextMessageAsync(
                chatId: chatId,
                text: "$\"☢  No malum buyruq kiritildi!",
                cancellationToken: cts);
        }
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
}