using ExpenseTracker.Application.Common.Constants;
using ExpenseTracker.Application.Common.Enums;
using ExpenseTracker.Application.Common.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Common.Helpers
{
    public static class ResponseHelper
    {
        public static ApiResponse<T> Success<T>(
            T data,
            string message = "Operation completed successfully.")
        {
            return new ApiResponse<T>
            {

                StatusCode = ApiStatusCodes.Success,
                ResponseType = ResponseType.Success,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Created<T>(
            T data,
            string message = "Resource created successfully.")
        {
            return new ApiResponse<T>
            {

                StatusCode = ApiStatusCodes.Created,
                ResponseType = ResponseType.Success,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Failure<T>(
            string message,
            int statusCode = ApiStatusCodes.BadRequest)
        {
            return new ApiResponse<T>
            {

                StatusCode = statusCode,
                ResponseType = ResponseType.Error,
                Message = message
            };
        }

        public static ApiResponse<T> ValidationFailure<T>(
            List<string> errors,
            string message = "Validation failed.")
        {
            return new ApiResponse<T>
            {

                StatusCode = ApiStatusCodes.ValidationError,
                ResponseType = ResponseType.Validation,
                Message = message,
                Errors = errors
            };
        }

        public static ApiResponse<T> NotFound<T>(
            string message = "Resource not found.")
        {
            return new ApiResponse<T>
            {

                StatusCode = ApiStatusCodes.NotFound,
                ResponseType = ResponseType.NotFound,
                Message = message
            };
        }

        public static ApiResponse<T> Unauthorized<T>(
            string message = "Unauthorized.")
        {
            return new ApiResponse<T>
            {

                StatusCode = ApiStatusCodes.Unauthorized,
                ResponseType = ResponseType.Unauthorized,
                Message = message
            };
        }
    }
}
