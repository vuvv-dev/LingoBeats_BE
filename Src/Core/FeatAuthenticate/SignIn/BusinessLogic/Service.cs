using FCommon.AccessToken;
using FCommon.Constants;
using FCommon.FeatureService;
using FCommon.RefreshToken;
using SignIn.Common;
using SignIn.DataAccess;
using SignIn.Models;

namespace SignIn.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IAppRefreshTokenHandler> _appRefreshTokenHandler;
    private readonly Lazy<IAppAccessTokenHandler> _appAccessTokenHandler;

    public Service(
        Lazy<IRepository> repository,
        Lazy<IAppRefreshTokenHandler> appRefreshTokenHandler,
        Lazy<IAppAccessTokenHandler> appAccessTokenHandler
    )
    {
        _repository = repository;
        _appRefreshTokenHandler = appRefreshTokenHandler;
        _appAccessTokenHandler = appAccessTokenHandler;
    }

    public async Task<AppResponseModel> ExecuteAsync(
        AppRequestModel request,
        CancellationToken cancellationToken
    )
    {
        var isUserExisted = await _repository.Value.DoesUserExistAsync(
            request.Email,
            cancellationToken
        );

        if (!isUserExisted)
        {
            return new() { AppCode = Constant.AppCode.USER_NOT_FOUND };
        }

        var isUserVerified = await _repository.Value.IsUserVerifiedAsync(
            request.Email,
            cancellationToken
        );

        if (!isUserVerified)
        {
            return new() { AppCode = Constant.AppCode.EMAIL_NOT_VERIFIED };
        }

        var passwordSignInResult = await _repository.Value.CheckPasswordSignInAsync(
            request.Email,
            request.Password,
            cancellationToken
        );

        if (!passwordSignInResult.IsSuccess)
        {
            if (passwordSignInResult.IsLockedOut)
            {
                return Constant.DefaultResponse.App.TEMPORARY_BANNED;
            }
            return Constant.DefaultResponse.App.PASSWORD_IS_INCORRECT;
        }

        var tokenId = Guid.NewGuid().ToString();
        var newRefreshToken = InitNewRefreshToken(
            passwordSignInResult.UserId,
            tokenId,
            request.RememberMe
        );

        var result = await _repository.Value.CreateRefreshTokenAsync(
            newRefreshToken,
            cancellationToken
        );
        if (!result)
        {
            return Constant.DefaultResponse.App.SERVER_ERROR;
        }

        var newAccessToken = _appAccessTokenHandler.Value.GenerateJWT(
            [
                new(AppContant.JsonWebToken.ClaimType.JTI, tokenId.ToString()),
                new(AppContant.JsonWebToken.ClaimType.SUB, passwordSignInResult.UserId.ToString()),
                new(
                    AppContant.JsonWebToken.ClaimType.PURPOSE.Name,
                    AppContant.JsonWebToken.ClaimType.PURPOSE.Value.USER_IN_APP
                ),
            ],
            Constant.APP_USER_ACCESS_TOKEN.DURATION_IN_MINUTES
        );

        return new()
        {
            AppCode = Constant.AppCode.SUCCESS,
            Body = new AppResponseModel.BodyModel()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Value,
            },
        };
    }

    private RefreshTokenModel InitNewRefreshToken(Guid userId, string tokenId, bool isRememberMe)
    {
        return new()
        {
            LoginProvider = tokenId,
            UserId = userId,
            Value = _appRefreshTokenHandler.Value.GenerateRefreshToken(),
            ExpiredAt = isRememberMe
                ? DateTime.UtcNow.AddDays(
                    Constant.APP_USER_REFRESH_TOKEN.DURATION_IN_MINUTES.REMEMBER_ME
                )
                : DateTime.UtcNow.AddDays(
                    Constant.APP_USER_REFRESH_TOKEN.DURATION_IN_MINUTES.NOT_REMEMBER_ME
                ),
            Name = Constant.APP_USER_REFRESH_TOKEN.NAME,
        };
    }
}
