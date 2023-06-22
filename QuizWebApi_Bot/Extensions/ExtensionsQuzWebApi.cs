using QuizWebApi_Bot.HelperServices;
using QuizWebApi_Bot.Interfaces;
using QuizWebApi_Bot.Repositories;

namespace QuizWebApi_Bot.Extensions;

public static class ExtensionsQuzWebApi
{
    public static void AddQuizWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
           .AddNewtonsoftJson();

        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();

        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileManager, FileManager>();
        services.AddScoped<IQuestionManger, QuestionManger>();

        services.AddHostedService<HandleBackgroundService>();
    }
}