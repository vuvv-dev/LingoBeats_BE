using Base.Mail.Config;

namespace Base.Mail.Handler;

public interface IEmailSendingHandler
{
    Task<bool> SendAsync(AddMailContent mailContent, CancellationToken cancellationToken);
}
