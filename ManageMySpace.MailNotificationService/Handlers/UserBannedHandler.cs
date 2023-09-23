using System.Threading.Tasks;
using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.MailNotificationService.Core;
using MimeKit;
using Serilog;

namespace ManageMySpace.MailNotificationService.Handlers
{
    public class UserBannedHandler : IEventHandler<UserBanned>
    {
        private readonly IMessageSender _sender;
        private readonly MessageSenderSettings _settings;

        public UserBannedHandler(IMessageSender sender, MessageSenderSettings settings)
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task HandleAsync(UserBanned @event)
        {
            Log.ForContext(nameof(UserBanned), @event, true).Information("Message has been received.");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _settings.From,
                _settings.FromEmail));

            message.To.Add(new MailboxAddress(@event.BannedUserName, @event.BannedUserEmail));

            message.Subject = "Account has been banned in the ManageMySpace system";

            message.Body = new TextPart("plain")
            {
                Text = $"Hi {@event.BannedUserName}!\n" +
                       "Your account in ManageMySpace system has been banned.\n" +
                       "You could not longer log in into the system.\n" +
                       "Contact support team.\n"
            };

            await _sender.SendMessageAsync(message);

            Log.ForContext(nameof(UserBanned), @event, true).Information("Email about user ban has been sent.");
        }
    }
}
