using SignIn.Models;

namespace SignIn.DataAccess;

public interface IRepository
{
    Task<bool> DoesUserExistAsync(string email, CancellationToken cancellationToken);
    Task<bool> IsUserVerifiedAsync(string email, CancellationToken cancellationToken);
    Task<PasswordSignInResultModel> CheckPasswordSignInAsync(
        string email,
        string password,
        CancellationToken cancellationToken
    );
    Task<bool> CreateRefreshTokenAsync(
        RefreshTokenModel model,
        CancellationToken cancellationToken
    );
}
