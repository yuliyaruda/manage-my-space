using System.Threading.Tasks;
using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.ActivityEvents;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.MailNotificationService.Core;
using MimeKit;
using Serilog;

namespace ManageMySpace.MailNotificationService.Handlers
{
    public class ActivitySubscribedHandler : IEventHandler<ActivitySubscribed>
    {
        private readonly IMessageSender _sender;
        private readonly MessageSenderSettings _settings;

        public ActivitySubscribedHandler(IMessageSender sender, MessageSenderSettings settings)
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task HandleAsync(ActivitySubscribed @event)
        {
            Log.ForContext(nameof(ActivitySubscribed), @event, true).Information("Message has been received.");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _settings.From,
                _settings.FromEmail));

            message.To.Add(new MailboxAddress(@event.UserEmail));

            message.Subject = $"You have subscribed to {@event.ActivityName}";

            message.Body = new TextPart("plain")
            {
                Text = "Hi!\n" +
                       $"You have been successfully subscribed to event: {@event.ActivityName}.\n" +
                       $"We are logging forward to you there!"
            };
            await _sender.SendMessageAsync(message);

            Log.ForContext(nameof(ActivitySubscribed), @event, true).Information("Email about the subscription to event has been sent.");
        }
    }
}
