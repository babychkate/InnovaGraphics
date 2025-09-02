using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace InnovaGraphics.Utils.Facade
{
    public class SendMessageSubsystem
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendMessageSubsystem> _logger;

        public SendMessageSubsystem(IConfiguration configuration, ILogger<SendMessageSubsystem> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();
        }

        public async Task SendVerificationCodeAsync(string email, string code)
        {
            var subject = "Верифікація";
            var body = $"Ваш код підтвердження: {code}";
            await SendEmailAsync(email, subject, body);
            _logger.LogInformation($"Код підтвердження надіслано на {email}. {code}");
        }

        public async Task SendLinkAsync(string email, string link)
        {
            var subject = "Перевірка або дія";
            var body = $"Перейдіть за посиланням для завершення дії: {link}";
            await SendEmailAsync(email, subject, body);
            _logger.LogInformation($"Посилання надіслано на {email}. {link}");
        }

        private async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var sender = _configuration["EmailSettings:Sender"];
            var password = _configuration["EmailSettings:Password"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("InnovaGraphics", sender));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(sender, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
