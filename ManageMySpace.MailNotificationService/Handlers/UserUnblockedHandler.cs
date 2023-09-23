using System.Threading.Tasks;
using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.MailNotificationService.Core;
using MimeKit;
using Serilog;

namespace ManageMySpace.MailNotificationService.Handlers
{
    public class UserUnblockedHandler : IEventHandler<UserUnblocked>
    {
        private readonly IMessageSender _sender;
        private readonly MessageSenderSettings _settings;

        public UserUnblockedHandler(IMessageSender sender, MessageSenderSettings settings)
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task HandleAsync(UserUnblocked @event)
        {
            Log.ForContext(nameof(UserUnblocked), @event, true).Information("Message has been received.");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _settings.From,
                _settings.FromEmail));

            message.To.Add(new MailboxAddress(@event.BannedUserName, @event.BannedUserEmail));

            message.Subject = "Account has been unblocked in the ManageMySpace system";

            message.Body = new TextPart("plain")
            {
                Text = $"Hi {@event.BannedUserName}!\n" +
                       "Your account in ManageMySpace system has been unblocked.\n" +
                       "You could continue using your account.\n" +
                       "Congratulations!!!\n"
            };

            await _sender.SendMessageAsync(message);

            Log.ForContext(nameof(UserUnblocked), @event, true).Information("Email about cancellation of the user ban has been sent.");
        }
    }
}
