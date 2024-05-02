using Microsoft.Data.SqlClient;
public class CustomException : Exception
{
    public CustomException() : base() { }
    public CustomException(Exception ex) : base(ex.Message, ex) { }
    public CustomException(string message) : base(message) { }

    public CustomException(string message, Exception innerException) : base(message, innerException) { }

    public static CustomException FromSqlException(SqlException ex)
    {

        return new CustomException(ex.Message, ex);
    }

    public static CustomException FromException(Exception ex)
    {
        return new CustomException(ex.Message, ex);
    }
}