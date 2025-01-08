using System.Net;
using System.Net.Mail;

namespace Extension;

public class EmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly string _senderEmail;
    
    public EmailSender(string smtpServer, int smtpPort, string senderEmail, string senderPassword, bool enableSsl = true)
    {
        _smtpClient = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = enableSsl
        };
        _senderEmail = senderEmail;
    }
    
    public void SendEmail(string recipientEmail, string subject, string body, bool isHtml = false)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            mailMessage.To.Add(recipientEmail);

            _smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error whilst trying to send an email: {ex.Message}");
        }
    }
}