using Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<string> SendCode(string email)
    {
        var random = new Random();
        var code = random.Next(1000, 9999).ToString();

        await SendEmailAsync(email,"Email Verification Code",
            $"Your Code: {code}");

        return code;
    }

    public async Task SendWelcomeEmail(string email, string name)
    {
        var body = $@"
                <h1>Welcome, {name}!</h1>
                <p>Your account has been created successfully.</p>
                <p>Thank you for using us!</p>
            ";

        await SendEmailAsync(email, "Welcome to TaskFlow", body);
    }

    public async Task SendEmail(string email, string subject, string body)
    {
        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            Console.WriteLine($"Email has sent successfully: {toEmail}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email sending error: {ex.Message}");
            throw;
        }
    }
}