using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor to initialize the EmailService with configuration settings.
    /// </summary>
    /// <param name="configuration">Configuration object to read email settings from appsettings.json.</param>
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Sends an email asynchronously using SMTP settings.
    /// </summary>
    /// <param name="toEmail">Recipient's email address.</param>
    /// <param name="subject">Subject of the email.</param>
    /// <param name="body">Email content in HTML format.</param>
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        // Retrieve email settings from the configuration file (appsettings.json)
        var emailSettings = _configuration.GetSection("EmailSettings");

        // Create a new email message
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Hello Greeting App", emailSettings["SenderEmail"])); // Sender email
        email.To.Add(new MailboxAddress("", toEmail)); // Recipient email
        email.Subject = subject; // Email subject
        email.Body = new TextPart("html") { Text = body }; // Email body (HTML content)

        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            // Connect to the SMTP server with TLS security
            await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), SecureSocketOptions.StartTls);

            // Authenticate using sender's email and password
            await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);

            // Send the email
            await smtp.SendAsync(email);

            // Disconnect from the SMTP server after sending the email
            await smtp.DisconnectAsync(true);
        }
    }
}