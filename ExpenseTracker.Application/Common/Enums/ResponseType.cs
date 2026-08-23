using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Common.Enums
{
    public enum ResponseType
    {
        Success,
        Error,
        Validation,
        NotFound,
        Unauthorized,
        Forbidden
    }
}
