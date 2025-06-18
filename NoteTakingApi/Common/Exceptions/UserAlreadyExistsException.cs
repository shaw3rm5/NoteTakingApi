namespace NoteTakingApi.Common.Exceptions;

public class UserAlreadyExistsException : ApplicationException
{
    public UserAlreadyExistsException(ErrorCodes errorCode, string message) : base(errorCode, message) { }
}