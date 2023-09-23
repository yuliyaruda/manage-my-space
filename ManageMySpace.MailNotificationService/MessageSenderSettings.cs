namespace ManageMySpace.MailNotificationService
{
    public class MessageSenderSettings
    {
        public string From { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string SmtpServerHostName { get; set; }
        public int SmtpServerPort { get; set; }
        public bool UseSsl { get; set; }
    }
}
