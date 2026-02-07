using AnimalFarm.Application.Interfaces;

namespace AnimalFarm.Infrastructure.Services;

public class ConsoleEmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        Console.WriteLine("========== EMAIL ==========");
        Console.WriteLine($"To:      {to}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body:    {body}");
        Console.WriteLine("===========================");
        return Task.CompletedTask;
    }
}
