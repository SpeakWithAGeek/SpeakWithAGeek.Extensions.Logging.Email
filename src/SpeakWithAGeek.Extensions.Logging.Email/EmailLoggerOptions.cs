namespace SpeakWithAGeek.Extensions.Logging.Email
{
    public class EmailLoggerOptions
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string EnvironmentName { get; set; }
    }
}
