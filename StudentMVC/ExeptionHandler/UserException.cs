using System.Data.SqlClient;

public class ExceptionHandler : Exception
{
    public ExceptionHandler() : base() { }
    public ExceptionHandler(Exception ex) : base(ex.Message, ex) { }
    public ExceptionHandler(string message) : base(message) { }

    public ExceptionHandler(string message, Exception innerException) : base(message, innerException) { }

    public static ExceptionHandler FromSqlException(SqlException ex)
    {

        return new ExceptionHandler(ex.Message, ex);
    }

    public static ExceptionHandler FromException(Exception ex)
    {
        return new ExceptionHandler(ex.Message, ex);
    }
}