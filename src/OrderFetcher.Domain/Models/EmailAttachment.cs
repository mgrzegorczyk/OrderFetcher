namespace OrderFetcher.Domain.Models;

public class EmailAttachment
{
    public string FileName { get; set; }
    public byte[] Content { get; set; }
}