namespace Scheduler.API.Common;
public enum ErrorType
{
    None = 0,
    Validation = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409,
    Unexpected = 500
}