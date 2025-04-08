using System.Data;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignIn.Models;

namespace SignIn.DataAccess;

public sealed class Repository : IRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly Lazy<UserManager<IdentityUserEntity>> _userManager;
    private readonly Lazy<SignInManager<IdentityUserEntity>> _signInManager;

    public Repository(
        AppDbContext appDbContext,
        Lazy<UserManager<IdentityUserEntity>> userManager,
        Lazy<SignInManager<IdentityUserEntity>> signInManager
    )
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<PasswordSignInResultModel> CheckPasswordSignInAsync(
        string email,
        string password,
        CancellationToken cancellationToken
    )
    {
        var foundUser = await _userManager.Value.FindByEmailAsync(email);
        var passwordValidatingResult = await _signInManager.Value.CheckPasswordSignInAsync(
            foundUser,
            password,
            lockoutOnFailure: true
        );

        return new()
        {
            IsSuccess = passwordValidatingResult.Succeeded,
            IsLockedOut = passwordValidatingResult.IsLockedOut,
            UserId = foundUser.Id,
        };
    }

    public async Task<bool> CreateRefreshTokenAsync(
        RefreshTokenModel model,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await _appDbContext
                .Set<IdentityUserTokenEntity>()
                .AddAsync(
                    new()
                    {
                        LoginProvider = model.LoginProvider,
                        Name = model.Name,
                        UserId = model.UserId,
                        ExpireAt = model.ExpiredAt,
                        Value = model.Value,
                    },
                    cancellationToken
                );
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DoesUserExistAsync(string email, CancellationToken cancellationToken)
    {
        var upperEmail = email.ToUpperInvariant();
        return await _appDbContext
            .Set<IdentityUserEntity>()
            .AnyAsync(
                predicate: user => user.NormalizedEmail!.Equals(upperEmail),
                cancellationToken
            );
    }

    public async Task<bool> IsUserVerifiedAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.Value.Users.FirstOrDefaultAsync(
            u => u.NormalizedEmail == email.ToUpperInvariant(),
            cancellationToken
        );

        if (user == null)
            return false;

        return await _userManager.Value.IsEmailConfirmedAsync(user);
    }
}
