using Serilog;
using Serilog.Events;

namespace QuizWebApi_Bot.HelperServices;

public class CustomLogger
{
    private const string FolderName = "Loggers";
    public static Serilog.Core.Logger WriteLogToFileSendToTelegram(IConfiguration configuration, string fileName = "logger.txt")
    {
        var botToken = configuration["ConnectionToTelegram:Bot_Token_Logger"]!;
        var chatId = long.Parse(configuration["ConnectionToTelegram:MY_ChatID"]!);

        var logger = new LoggerConfiguration()
            .WriteTo.File(Path.Combine(FolderName, fileName), LogEventLevel.Error)
            .WriteTo.Telegram(botToken, chatId.ToString(), restrictedToMinimumLevel: LogEventLevel.Error)
            .CreateLogger();

        return logger;
    }
}

