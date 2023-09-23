using System.Threading.Tasks;
using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.MailNotificationService.Core;
using MimeKit;
using Serilog;

namespace ManageMySpace.MailNotificationService.Handlers
{
    public class UserCreatedHandler : IEventHandler<UserCreated>
    {
        private readonly IMessageSender _sender;
        private readonly MessageSenderSettings _settings;

        public UserCreatedHandler(IMessageSender sender, MessageSenderSettings settings)
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task HandleAsync(UserCreated @event)
        {
            Log.ForContext(nameof(UserCreated), @event, true).Information("Message has been received.");

            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress(
                _settings.From,
                _settings.FromEmail));

            message.To.Add(new MailboxAddress(
                @event.Name,
                @event.Email));

            message.Subject = "New account in ManageMySpace system";
            
            message.Body = new TextPart("plain")
            {
                Text = $"Hi {@event.Name}!\n" +
                       "Your account has been successfully created!\n" +
                       "If you have any questions feel free to contact us!\n"
            };

            await _sender.SendMessageAsync(message);

            Log.ForContext(nameof(UserCreated), @event, true).Information("Email about user registration has been sent.");
        }
    }
}
