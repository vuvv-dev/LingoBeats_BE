using Base.Config;
using Base.Mail.Config;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Base.Mail.Handler;

public class GoogleEmailSendingHandler : IEmailSendingHandler
{
    private readonly EmailOption _googleGmailSendingOption;

    public GoogleEmailSendingHandler(EmailOption googleGmailSendingOption)
    {
        _googleGmailSendingOption = googleGmailSendingOption;
    }

    public async Task<bool> SendAsync(
        AddMailContent mailContent,
        CancellationToken cancellationToken
    )
    {
        if (Equals(objA: mailContent, objB: default))
        {
            return false;
        }

        MimeMessage email =
            new()
            {
                Sender = new(
                    name: _googleGmailSendingOption.Sender,
                    address: _googleGmailSendingOption.Username
                ),
                From =
                {
                    new MailboxAddress(
                        name: _googleGmailSendingOption.Sender,
                        address: _googleGmailSendingOption.Username
                    ),
                },
                To = { MailboxAddress.Parse(text: mailContent.To) },
                Subject = mailContent.Subject,
                Body = new BodyBuilder { HtmlBody = mailContent.Body }.ToMessageBody(),
            };
        try
        {
            using SmtpClient smtp = new();

            await smtp.ConnectAsync(
                host: _googleGmailSendingOption.SmtpServer,
                port: _googleGmailSendingOption.Port,
                options: SecureSocketOptions.StartTlsWhenAvailable,
                cancellationToken: cancellationToken
            );

            await smtp.AuthenticateAsync(
                userName: _googleGmailSendingOption.Username,
                password: _googleGmailSendingOption.Password,
                cancellationToken: cancellationToken
            );

            await smtp.SendAsync(message: email, cancellationToken: cancellationToken);

            await smtp.DisconnectAsync(quit: true, cancellationToken: cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
