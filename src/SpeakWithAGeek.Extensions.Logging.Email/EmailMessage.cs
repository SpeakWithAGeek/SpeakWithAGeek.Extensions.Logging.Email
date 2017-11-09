namespace SpeakWithAGeek.Extensions.Logging.Email
{
    public class EmailMessage
    {
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public string SenderName { get; set; }
        public string RecipientName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
