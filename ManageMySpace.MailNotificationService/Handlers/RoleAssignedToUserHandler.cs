using System.Threading.Tasks;
using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.MailNotificationService.Core;
using MimeKit;
using Serilog;

namespace ManageMySpace.MailNotificationService.Handlers
{
    public class RoleAssignedToUserHandler : IEventHandler<RoleAssignedToUser>
    {
        private readonly IMessageSender _sender;
        private readonly MessageSenderSettings _settings;

        public RoleAssignedToUserHandler(IMessageSender sender, MessageSenderSettings settings)
        {
            _sender = sender;
            _settings = settings;
        }

        public async Task HandleAsync(RoleAssignedToUser @event)
        {
            Log.ForContext(nameof(RoleAssignedToUser), @event, true).Information("Message has been received.");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _settings.From,
                _settings.FromEmail));

            message.To.Add(new MailboxAddress(@event.UserName,@event.UserEmail));

            message.Subject = "You have been assigned a new role in the ManageMySpace system";

            message.Body = new TextPart("plain")
            {
                Text = $"Hi {@event.UserName}!\n" +
                       "Your role in ManageMySpace system has been changed.\n" +
                       $"Now you are {@event.RoleName}\n" +
                       "Congratulations!!!"
            };

            await _sender.SendMessageAsync(message);

            Log.ForContext(nameof(RoleAssignedToUser), @event, true).Information("Email about new role of the user has been sent.");
        }
    }
}
