using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Microsoft.Extensions.Configuration;

namespace AuthService;

public class ForgotPasswordEventHandler : IIntegrationEventHandler<ForgotPasswordEvent, (string, ResultType)>
{
    private readonly IConfiguration _configuration;
    private readonly IDatabaseHandler _dbHandler;
    private readonly EmailSender _emailSender;

    public ForgotPasswordEventHandler(IConfiguration configuration, IDatabaseHandler dbHandler)
    {
        _configuration = configuration;
        _dbHandler = dbHandler;
        
        string smtpServer = _configuration["SmtpSettings:Server"];
        int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        string senderEmail = _configuration["SmtpSettings:SenderEmail"];
        string senderPassword = _configuration["SmtpSettings:SenderPassword"];

        _emailSender = new EmailSender(smtpServer, smtpPort, senderEmail, senderPassword);
    }
    
    public async Task<(string, ResultType)> HandleAsync(ForgotPasswordEvent @event)
    {
        string resetToken = Guid.NewGuid().ToString();
        DateTime now = DateTime.UtcNow;
        
        var reset = new PasswordReset
        {
            UserEmail = @event.Email,
            ResetToken = resetToken,
            CreatedAt = now,
            ExpiresAt = now.AddHours(1)
        };
        
        await _dbHandler.AddAsync(reset);

        // TODO : Real URL
        string resetLink = $"{_configuration["AppSettings:BaseUrl"]}/reset-password?token={resetToken}";

        string subject = "Password Reset Request";
        string body = $"Click the following link to reset your password: {resetLink}\n" +
                      "This link will expire in 1 hour.";

        _emailSender.SendEmail(@event.Email, subject, body, isHtml: false);

        return ("Email sent", ResultType.Success);
    }
}