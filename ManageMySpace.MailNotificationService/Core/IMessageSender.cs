using System.Threading.Tasks;
using MimeKit;

namespace ManageMySpace.MailNotificationService.Core
{
    public interface IMessageSender
    {
        Task SendMessageAsync(MimeMessage message);
    }
}
