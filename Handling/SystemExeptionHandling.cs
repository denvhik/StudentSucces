namespace  Handling;
public class SystemExeptionHandle: Exception
{
    public SystemExeptionHandle() : base() { }

    public SystemExeptionHandle(string message) : base(message) { }
    public SystemExeptionHandle(Exception ex) : base(ex.Message, ex) { }

    public SystemExeptionHandle(string message, Exception innerException) : base(message, innerException) { }

    public static SystemExeptionHandle FromSystemException(Exception ex)
    {
        return new SystemExeptionHandle(ex.Message, ex);
    }
}
