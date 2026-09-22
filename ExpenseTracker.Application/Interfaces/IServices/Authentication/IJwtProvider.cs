using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Interfaces.Services.Authentication
{
    public interface IJwtProvider
    {
        string GenerateToken(
      int userId,
      string userName,
      string email,
      IEnumerable<string> roles);
    }
}
