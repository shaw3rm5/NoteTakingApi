namespace NoteTakingApi.Common.Exceptions;

public class NotAuthorizedException : ApplicationException
{
    public NotAuthorizedException(ErrorCodes errorCode, string message) 
        : base(errorCode, message) { }
}