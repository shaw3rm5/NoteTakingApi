namespace NoteTakingApi.Common.Exceptions;

public abstract class ApplicationException : Exception
{
    public ErrorCodes ErrorCode { get; }
    protected ApplicationException(ErrorCodes errorCode, string  message) : base(message) 
    { ErrorCode = errorCode; }
}