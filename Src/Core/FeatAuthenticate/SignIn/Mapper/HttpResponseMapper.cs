using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using SignIn.Common;
using SignIn.Models;
using SignIn.Presentation;
using SignIn.Presentation.Filter.SetStageBag;

namespace SignIn.Mapper;

public static class HttpResponseMapper
{
    private static ConcurrentDictionary<
        Constant.AppCode,
        Func<AppRequestModel, AppResponseModel, HttpContext, Response>
    > _httpResponseMapper;

    private static void Init()
    {
        if (Equals(_httpResponseMapper, null))
        {
            _httpResponseMapper = new();
            _httpResponseMapper.TryAdd(
                Constant.AppCode.USER_NOT_FOUND,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.USER_NOT_FOUND;
                }
            );
            _httpResponseMapper.TryAdd(
                Constant.AppCode.EMAIL_NOT_VERIFIED,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.EMAIL_NOT_VERIFIED;
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.TEMPORARY_BANNED,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.TEMPORARY_BANNED;
                }
            );
            _httpResponseMapper.TryAdd(
                Constant.AppCode.PASSWORD_IS_INCORRECT,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.PASSWORD_IS_INCORRECT;
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.PASSWORD_IS_INVALID,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.PASSWORD_IS_INVALID;
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.SUCCESS,
                (appRequest, appResponse, httpContext) =>
                {
                    return new()
                    {
                        HttpCode = StatusCodes.Status200OK,
                        AppCode = Constant.AppCode.SUCCESS.ToString(),
                        Body = new()
                        {
                            AccessToken = appResponse.Body.AccessToken,
                            RefreshToken = appResponse.Body.RefreshToken,
                        },
                    };
                }
            );

            _httpResponseMapper.TryAdd(
                Constant.AppCode.SERVER_ERROR,
                (appRequest, appResponse, httpContext) =>
                {
                    return Constant.DefaultResponse.Http.SERVER_ERROR;
                }
            );
        }
    }

    public static Response Get(
        AppRequestModel request,
        Models.AppResponseModel response,
        HttpContext context
    )
    {
        Init();
        var stageBag = context.Items[nameof(StageBag)] as StageBag;

        var httpResponse = _httpResponseMapper[response.AppCode](request, response, context);
        stageBag!.HttpResponse = httpResponse;

        return httpResponse;
    }
}
