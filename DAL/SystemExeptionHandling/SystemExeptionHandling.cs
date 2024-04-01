namespace DAL.SystemExeptionHandling;
public class SystemExeptionHandle: Exception
{
    public SystemExeptionHandle() : base() { }

    public SystemExeptionHandle(string message) : base(message) { }

    public SystemExeptionHandle(string message, Exception innerException) : base(message, innerException) { }

    public static SystemExeptionHandle FromSystemException(Exception ex)
    {
     
        return new SystemExeptionHandle("The operation failed. Please try again or contact your administrator.", ex);
    }
}
