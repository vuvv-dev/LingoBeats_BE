using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FCommon.Constants;
using Microsoft.IdentityModel.Tokens;

namespace FCommon.AccessToken;

public sealed class AppAccessTokenHandler : IAppAccessTokenHandler
{
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AppAccessTokenHandler(TokenValidationParameters tokenValidationParameters)
    {
        _tokenValidationParameters = tokenValidationParameters;
    }

    public string GenerateJWT(IEnumerable<Claim> claims, int additionalMinutesFromNow)
    {
        var signingCredentials = new SigningCredentials(
            _tokenValidationParameters.IssuerSigningKey,
            SecurityAlgorithms.HmacSha256
        );

        var currentTime = DateTime.UtcNow;
        var expirationTime = currentTime.AddMinutes(additionalMinutesFromNow);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _tokenValidationParameters.ValidAudience,
            Issuer = _tokenValidationParameters.ValidIssuer,
            IssuedAt = currentTime,
            Expires = expirationTime,
            NotBefore = expirationTime - TimeSpan.FromSeconds(1),
            TokenType = AppContant.JsonWebToken.Type.JWT,
            CompressionAlgorithm = CompressionAlgorithms.Deflate,
            SigningCredentials = signingCredentials,
            Subject = new(claims),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtDescriptor = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(jwtDescriptor);

        return jwt;
    }

    public static bool IsAccessTokenExpired(string expClaimValue)
    {
        var tokenExpireTime = DateTimeOffset
            .FromUnixTimeSeconds(long.Parse(expClaimValue))
            .UtcDateTime;

        return tokenExpireTime < DateTime.UtcNow;
    }
}
