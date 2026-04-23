namespace CLS.Common.Exceptions;

public class SlaViolationException : Exception
{
    public SlaViolationException(string message) : base(message) { }
}
