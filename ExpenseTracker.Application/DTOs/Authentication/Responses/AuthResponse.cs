using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.DTOs.Authentication.Responses
{

    public class AuthResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Token { get; set; }
    }
}
