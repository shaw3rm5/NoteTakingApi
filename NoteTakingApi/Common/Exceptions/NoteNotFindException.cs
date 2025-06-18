namespace NoteTakingApi.Common.Exceptions;

public class NoteNotFindException : ApplicationException
{
    public NoteNotFindException(ErrorCodes errorCode, string message) : base(errorCode, message) { }
}