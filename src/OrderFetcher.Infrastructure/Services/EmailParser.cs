using MimeKit;
using OrderFetcher.Application.Interfaces;
using OrderFetcher.Domain.Models;

namespace OrderFetcher.Infrastructure.Services;

public class EmailParser : IEmailParser
{
    public async Task<ParsedEmail> ParseEmailFromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file at path {filePath} does not exist.");
        }

        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 4096, useAsync: true);

        cancellationToken.ThrowIfCancellationRequested();

        var message = await MimeMessage.LoadAsync(stream, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return new ParsedEmail
        {
            Subject = message.Subject,
            Body = GetEmailBody(message),
            Attachments = GetAttachments(message)
        };
    }


    private string GetEmailBody(MimeMessage message)
    {
        if (message.TextBody != null)
        {
            return message.TextBody;
        }

        if (message.HtmlBody != null)
        {
            return message.HtmlBody;
        }

        return string.Empty;
    }

    private List<EmailAttachment> GetAttachments(MimeMessage message)
    {
        var attachments = new List<EmailAttachment>();

        foreach (var attachment in message.Attachments)
        {
            if (attachment is MimePart mimePart && mimePart.Content != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    mimePart.Content.DecodeTo(memoryStream);
                    attachments.Add(new EmailAttachment
                    {
                        FileName = mimePart.FileName,
                        Content = memoryStream.ToArray()
                    });
                }
            }
        }

        return attachments;
    }
}