namespace Base.Common.DependencyInjection;

internal interface IExternalServiceRegister
{
    IServiceCollection Register(IServiceCollection services, IConfiguration configuration);
}
