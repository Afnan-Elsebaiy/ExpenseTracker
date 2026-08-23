using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Common.Constants
{
    public static class ApiStatusCodes
    {
        public const int Success = 200;
        public const int Created = 201;
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int ValidationError = 422;
        public const int InternalServerError = 500;
    }
}
