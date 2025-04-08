using Base.Common.DependencyInjection;
using Base.Config;
using Base.Mail.Handler;

namespace Base.Mail;

internal sealed class RegistrationCenter : IExternalServiceRegister
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        ConfigGoogleSmtpMailSender(services, configuration);
        return services;
    }

    public static IServiceCollection ConfigGoogleSmtpMailSender(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        var smtpMailOption = configuration
            .GetRequiredSection(key: "EmailSettings")
            .Get<EmailOption>();

        services
            .AddSingleton<IEmailSendingHandler, GoogleEmailSendingHandler>()
            .MakeSingletonLazy<IEmailSendingHandler>();

        if (smtpMailOption != null)
        {
            services.AddSingleton(smtpMailOption);
        }

        return services;
    }
}
