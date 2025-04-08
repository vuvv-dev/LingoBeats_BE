using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignUp.Models;

namespace SignUp.DataAccess;

public interface IRepository
{
    Task<bool> DoesEmailExistedAsync(string email, CancellationToken ct);
    Task<bool> IsPasswordValidAsync(string email, string password, CancellationToken ct);
    Task<bool> CreateUserAsync(UserInformationModal user, CancellationToken ct);
}
