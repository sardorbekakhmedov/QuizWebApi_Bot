using QuizWebApi_Bot.Extensions;
using QuizWebApi_Bot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var logger = CustomLogger
    .WriteLogToFileSendToTelegram(builder.Configuration, "QuizWebApiLogger.txt");

builder.Logging.AddSerilog(logger);

builder.Services.AddQuizWebApiServices(builder.Configuration);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSwagger();
//app.UseSwaggerUI();

app.UseCustomQuizWebApiErrorMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
