using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SignIn.Presentation;

[ValidateNever]
public sealed class Request
{
    [DefaultValue("vuvo070403@gmail.com")]
    public string Email { get; set; }

    [DefaultValue("Admin123@")]
    public string Password { get; set; }

    [DefaultValue(false)]
    public bool RememberMe { get; set; }
}
