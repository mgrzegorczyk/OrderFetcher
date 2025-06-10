namespace OrderFetcher.Domain.Models;

public class ParsedEmail
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<EmailAttachment> Attachments { get; set; }
}