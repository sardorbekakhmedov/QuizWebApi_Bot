﻿using Microsoft.Extensions.Options;
using QuizWebApi_Bot.Entities;
using QuizWebApi_Bot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using InputFile = Telegram.Bot.Types.InputFile;

namespace QuizWebApi_Bot.HelperServices;

public class HandleBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly TelegramToken _token;

    public HandleBackgroundService(IServiceProvider services, IOptions<TelegramToken> token)
    {
        _services = services;
        _token = token.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            await SendMessageAsync(stoppingToken);
            Console.WriteLine("HandleBackgroundService is working!");
        }

        Console.WriteLine("HandleBackgroundService is stopping!");
    }

    private async Task SendMessageAsync(CancellationToken cancellationToken)
    {
        var botClient = new TelegramBotClient(_token.TgToken);

        await using AsyncServiceScope serviceScope = _services.CreateAsyncScope();

        var userService = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
        var questionService = serviceScope.ServiceProvider.GetRequiredService<IQuestionRepository>();

        var users = await userService.GetAllUsersAsync();

        foreach (var user in users)
        {
            Console.WriteLine("SendMessageAsync method is working!");
            var questionId = await SendQuestionAsync(user.UserId, botClient, serviceScope, questionService, cancellationToken);
            await userService.IncrementSentQuestionAsync(user.UserId);
            //  await userService.AddSentUserQuestionCollectionAsync(user, questionId, true);
        }
    }

    private async Task<Guid> SendQuestionAsync(long userChatId, ITelegramBotClient botClient,
        AsyncServiceScope serviceScope, IQuestionRepository questionService,
        CancellationToken cancellationToken)
    {
        var randomQuestion = await questionService.GetRandomQuestionAsync();

        Console.WriteLine("SendQuestionAsync method is working!");
        if (randomQuestion.ImagePath is not null)
        {
            await using var fileStream = File.OpenRead(randomQuestion.ImagePath);

            await botClient.SendPhotoAsync(
                chatId: userChatId,
                photo: InputFile.FromStream(fileStream),
                caption: randomQuestion.QuestionText,
                replyMarkup: GetInlineButtonForOptions(randomQuestion.Choices, randomQuestion.Id.ToString()),
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: userChatId,
                text: randomQuestion.QuestionText,
                replyMarkup: GetInlineButtonForOptions(randomQuestion.Choices, randomQuestion.Id.ToString()),
                cancellationToken: cancellationToken);
        }
        return randomQuestion.Id;
    }

    public InlineKeyboardMarkup GetInlineButtonForOptions(List<string> options, string questionId)
    {
        var inlineButtons = new List<List<InlineKeyboardButton>>();

        foreach (var data in options)
        {
            var button = new List<InlineKeyboardButton>()
            {
                InlineKeyboardButton.WithCallbackData(data, $"!_?answer?_!,{questionId},{data}")
            };

            inlineButtons.Add(button);
        }
        return new InlineKeyboardMarkup(inlineButtons);
    }

}