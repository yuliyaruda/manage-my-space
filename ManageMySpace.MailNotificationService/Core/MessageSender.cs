using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace ManageMySpace.MailNotificationService.Core
{
    public class MessageSender : IMessageSender
    {
        private MessageSenderSettings _settings;
        public MessageSender(MessageSenderSettings settings)
        {
            _settings = settings;
        }

        public async Task SendMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_settings.SmtpServerHostName, _settings.SmtpServerPort, _settings.UseSsl);
                await client.AuthenticateAsync(_settings.FromEmail, _settings.Password);
                await client.SendAsync(message);
            }
        }
    }
}
