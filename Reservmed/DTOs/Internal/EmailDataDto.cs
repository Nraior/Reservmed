namespace Reservmed.DTOs.Internal
{
    public class EmailDataDto
    {
        public required string EmailAddress { get; set; }
        public required string EmailContent { get; set; }
        public required string Subject { get; set; }
    }
}
