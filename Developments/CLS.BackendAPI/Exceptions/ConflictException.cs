namespace CLS.BackendAPI.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
