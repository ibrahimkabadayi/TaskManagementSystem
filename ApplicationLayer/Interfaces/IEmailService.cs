namespace Application.Interfaces;

public interface IEmailService
{
   Task<string> SendCode(string email);
   Task SendWelcomeEmail(string email, string name);
   Task SendEmail(string email, string subject, string body);
}