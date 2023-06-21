namespace QuizWebApi_Bot.Interfaces;

public interface IFileManager
{
    Task<string> SaveFileToWwwrootAsync(IFormFile logoFile, string folderName);
}