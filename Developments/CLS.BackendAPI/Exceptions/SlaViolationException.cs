namespace CLS.BackendAPI.Exceptions;

public class SlaViolationException : Exception
{
    public SlaViolationException(string message) : base(message) { }
}
