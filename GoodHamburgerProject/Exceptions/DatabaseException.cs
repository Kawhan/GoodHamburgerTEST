public class DatabaseException : Exception
{
    public string MethodName { get; }

    public DatabaseException(string methodName, string message, Exception innerException)
        : base(message, innerException)
    {
        MethodName = methodName;
    }
}