using Base.Config;
using FluentValidation;

namespace SignIn.Presentation.Validation;

public sealed class ValidationProfile : AbstractValidator<Request>
{
    public ValidationProfile(AspNetCoreIdentityOption aspNetCoreIdentityOption)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(aspNetCoreIdentityOption.Password!.RequiredLength);
    }
}
