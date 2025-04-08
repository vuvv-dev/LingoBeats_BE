using System.Data;
using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignUp.Models;

namespace SignUp.DataAccess;

public sealed class Repository : IRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly Lazy<UserManager<IdentityUserEntity>> _userManager;

    public Repository(AppDbContext appDbContext, Lazy<UserManager<IdentityUserEntity>> userManager)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
    }

    public async Task<bool> CreateUserAsync(UserInformationModal user, CancellationToken ct)
    {
        var dbResult = true;

        await _appDbContext
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var dbTransaction = await _appDbContext.Database.BeginTransactionAsync(
                    IsolationLevel.ReadUncommitted,
                    ct
                );
                try
                {
                    var newIdentityUser = new IdentityUserEntity
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.Email,
                        EmailConfirmed = user.IsEmailConfirmed,
                    };

                    var result = await _userManager.Value.CreateAsync(
                        newIdentityUser,
                        user.Password
                    );

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateException();
                    }

                    var additionUserInfo = new AdditionalUserInformationEntity
                    {
                        Id = user.Id,
                        FirstName = user.AdditionalUserInfor.FirstName,
                        LastName = user.AdditionalUserInfor.LastName,
                    };

                    await _appDbContext
                        .Set<AdditionalUserInformationEntity>()
                        .AddAsync(additionUserInfo, ct);
                    await _appDbContext.SaveChangesAsync(ct);
                    await dbTransaction.CommitAsync();
                }
                catch (DbUpdateException)
                {
                    await dbTransaction.RollbackAsync();
                    dbResult = false;
                }
            });
        return dbResult;
    }

    public Task<bool> DoesEmailExistedAsync(string email, CancellationToken ct)
    {
        var upperEmail = email.ToUpper();
        return _appDbContext
            .Set<IdentityUserEntity>()
            .AnyAsync(predicate: user => user.NormalizedEmail!.Equals(upperEmail), ct);
    }

    public async Task<bool> IsPasswordValidAsync(
        string email,
        string password,
        CancellationToken ct
    )
    {
        IdentityResult? result = default;

        foreach (var validator in _userManager.Value.PasswordValidators)
        {
            result = await validator.ValidateAsync(
                _userManager.Value,
                new() { Id = default },
                password
            );
        }

        if (Equals(result, default))
        {
            return true;
        }
        return result.Succeeded;
    }
}
