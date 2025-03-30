namespace Base.Common.DependencyInjection;

public interface IServiceRegister
{
    IServiceCollection Register(IServiceCollection services, IConfiguration configuration);
}
