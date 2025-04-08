namespace FCommon.RefreshToken;

public sealed class AppRefreshTokenHandler : IAppRefreshTokenHandler
{
    public string GenerateRefreshToken(int tokenLenght = 36)
    {
        return Guid.NewGuid().ToString();
    }
}
