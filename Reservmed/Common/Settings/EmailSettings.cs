namespace Reservmed.Common.Settings
{
    public class EmailSettings
    {
        public required string SenderName { get; set; }
        public required string SenderEmail { get; set; }
        public required string SenderUsername { get; set; }
        public required string SenderPassword { get; set; }
        public required string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

    }
}
