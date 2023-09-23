using System.Threading.Tasks;
using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.ActivityEvents;
using ManageMySpace.MailNotificationService.Core;
using MimeKit;
using Serilog;

namespace ManageMySpace.MailNotificationService.Handlers
{
    public class ActivityCanceledHandler : IEventHandler<ActivityCanceled>
    {
        private readonly IMessageSender _sender;
        private readonly MessageSenderSettings _settings;

        public ActivityCanceledHandler(IMessageSender sender, MessageSenderSettings settings)
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task HandleAsync(ActivityCanceled @event)
        {
            Log.ForContext(nameof(ActivityCanceled), @event, true).Information("Message has been received.");

            foreach (var visitor in @event.VisitorUserEmails)
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(
                    _settings.From,
                    _settings.FromEmail));

                message.To.Add(new MailboxAddress(visitor));

                message.Subject = $"Event {@event.ActivityName} has been canceled.";

                message.Body = new TextPart("plain")
                {
                    Text = "Hi!\n" +
                           $"Unfortunately event: {@event.ActivityName} what should occur at { @event.ActivityDate } has been canceled.\n" +
                           "Check ManageMySpace site to find new interesting event for you.\n"
                };

                await _sender.SendMessageAsync(message);

                Log.ForContext(nameof(ActivityCanceled), @event, true).Information("Email about the event cencelation has been sent.");
            }
        }
    }
}
