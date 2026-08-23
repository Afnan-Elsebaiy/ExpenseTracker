using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Validators
{
    public class ValidationError
    {
        public string PropertyName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
