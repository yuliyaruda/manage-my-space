using System.Threading.Tasks;
using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.ActivityEvents;
using ManageMySpace.MailNotificationService.Core;
using MimeKit;
using Serilog;

namespace ManageMySpace.MailNotificationService.Handlers
{
    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        private readonly IMessageSender _sender;
        private readonly MessageSenderSettings _settings;

        public ActivityCreatedHandler(IMessageSender sender, MessageSenderSettings settings)
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task HandleAsync(ActivityCreated @event)
        {
            Log.ForContext(nameof(ActivityCreated), @event, true).Information("Message has been received.");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _settings.From,
                _settings.FromEmail));

            message.To.Add(new MailboxAddress(@event.UserEmail));

            message.Subject = $"Event {@event.ActivityName} has been created.";

            message.Body = new TextPart("plain")
            {
                Text = "Hi!\n" +
                       $"Event {@event.ActivityName} haw been successfully created.\n" +
                       $"Thank you for your activity!"
            };
            await _sender.SendMessageAsync(message);

            Log.ForContext(nameof(ActivityCreated), @event, true).Information("Email about the event creation has been sent.");
        }
    }
}
