using FCommon.Constants;

namespace FCommon.RefreshToken;

public interface IAppRefreshTokenHandler
{
    string GenerateRefreshToken(int tokenLenght = AppContant.REFRESH_TOKEN_LENGTH);
}
